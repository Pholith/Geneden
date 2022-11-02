using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
// Code provenant de https://github.com/redbluegames/unity-notnullattribute


/// <summary>
/// Not Null Attribute will error in the Editor if an object field has not been assigned.
/// </summary>
[AttributeUsage(System.AttributeTargets.Field)]
public class NotNullAttribute : PropertyAttribute
{
#if UNITY_EDITOR

    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="NotNullAttribute"/> should ignore prefabs.
    /// Use this when your object reqiures a reference to an object in the scene, such as a spawn point.
    /// </summary>
    /// <value><c>true</c> if ignore prefab; otherwise, <c>false</c>.</value>
    public bool IgnorePrefab { get; set; }
}

/// <summary>
/// Not null violation represents data for objects that do not have their required (NotNull) fields
/// assigned.
/// </summary>
public class NotNullViolation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedBlueGames.NotNull.NotNullViolation"/> class.
    /// </summary>
    /// <param name="fieldInfo">Field info that describes the NotNull field.</param>
    /// <param name="sourceMB">Source MonoBehavior that contains the field.</param>
    public NotNullViolation(FieldInfo fieldInfo, MonoBehaviour sourceMB)
    {
        FieldInfo = fieldInfo;
        SourceMonoBehaviour = sourceMB;
        ErrorGameObject = sourceMB.gameObject;
    }

    /// <summary>
    /// Gets or sets the field info associated with the NotNull attribute.
    /// </summary>
    /// <value>The field info.</value>
    public FieldInfo FieldInfo { get; set; }

    /// <summary>
    /// Gets or sets the game object that contains the component with the violation.
    /// </summary>
    /// <value>The erroring game object.</value>
    public GameObject ErrorGameObject { get; set; }

    /// <summary>
    /// Gets or sets the MonoBehavior that contains the violation.
    /// </summary>
    /// <value>The source mono behaviour.</value>
    public MonoBehaviour SourceMonoBehaviour { get; set; }

    /// <summary>
    /// Gets the full path to the erroring game object, including parents.
    /// </summary>
    /// <value>The full name.</value>
    public string FullName
    {
        get
        {
            Transform currentParent = ErrorGameObject.transform.parent;
            string fullName = ErrorGameObject.name;
            while (currentParent != null)
            {
                fullName = currentParent.gameObject.name + "/" + fullName;
                currentParent = currentParent.transform.parent;
            }

            return fullName;
        }
    }

    /// <summary>
    /// Returns a <see cref="string"/> that represents the current 
    /// <see cref="RedBlueGames.NotNull.NotNullViolation"/>.
    /// </summary>
    /// <returns>A <see cref="string"/> that represents the current 
    /// <see cref="RedBlueGames.NotNull.NotNullViolation"/>.</returns>
    public override string ToString()
    {
        return string.Format("[NotNullViolation: Field={0}, FullName={1}]", FieldInfo.Name, FullName);
    }

    /// <summary>
    /// Utility methods to help with Reflection
    /// </summary>
    public static class ReflectionUtility
    {
        /// <summary>
        /// Gets all fields in a class that have a specified attribute. Default returns only public fields. Use binding flags
        /// to get NonPublic fields.
        /// </summary>
        /// <returns>A List of FieldInfo for all fields with the specified attribute.</returns>
        /// <param name="classToInspect">Class to inspect.</param>
        /// <param name="reflectionFlags">Reflection flags - supplying none uses default GetFields method.</param>
        /// <typeparam name="T">The Attribute type to search for.</typeparam>
        public static List<FieldInfo> GetFieldsWithAttributeFromType<T>(
            Type classToInspect,
            BindingFlags reflectionFlags = BindingFlags.Default)
        {
            List<FieldInfo> fieldsWithAttribute = new List<FieldInfo>();
            FieldInfo[] allFields;
            if (reflectionFlags == BindingFlags.Default)
            {
                allFields = classToInspect.GetFields();
            }
            else
            {
                allFields = classToInspect.GetFields(reflectionFlags);
            }

            foreach (FieldInfo fieldInfo in allFields)
            {
                foreach (Attribute attribute in Attribute.GetCustomAttributes(fieldInfo))
                {
                    if (attribute.GetType() == typeof(T))
                    {
                        fieldsWithAttribute.Add(fieldInfo);
                        break;
                    }
                }
            }

            return fieldsWithAttribute;
        }
    }

    /// <summary>
    /// This class is responsible for checking objects for NotNull violations.
    /// </summary>
    public class NotNullChecker
    {
        /// <summary>
        /// Finds the erroring NotNull fields on a GameObject.
        /// </summary>
        /// <returns>The erroring fields.</returns>
        /// <param name="sourceObject">Source object.</param>
        public static List<NotNullViolation> FindErroringFields(GameObject sourceObject)
        {
            List<NotNullViolation> erroringFields = new List<NotNullViolation>();
            MonoBehaviour[] monobehaviours = sourceObject.GetComponents<MonoBehaviour>();
            for (int i = 0; i < monobehaviours.Length; i++)
            {
                try
                {
                    if (MonoBehaviourHasErrors(monobehaviours[i]))
                    {
                        List<NotNullViolation> violationsOnMonoBehaviour = FindErroringFields(monobehaviours[i]);
                        erroringFields.AddRange(violationsOnMonoBehaviour);
                    }
                }
                catch (System.ArgumentNullException)
                {
                    // TODO: Handle missing monobehaviours
                }
            }

            return erroringFields;
        }

        private static List<NotNullViolation> FindErroringFields(MonoBehaviour sourceMB)
        {
            if (sourceMB == null)
            {
                throw new System.ArgumentNullException("MonoBehaviour is null. It likely references" +
                    " a script that's been deleted.");
            }

            List<NotNullViolation> erroringFields = new List<NotNullViolation>();

            // Add null NotNull fields
            List<FieldInfo> notNullFields =
                ReflectionUtility.GetFieldsWithAttributeFromType<NotNullAttribute>(
                    sourceMB.GetType(),
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            foreach (FieldInfo notNullField in notNullFields)
            {
                object fieldObject = notNullField.GetValue(sourceMB);
                if (fieldObject == null || fieldObject.Equals(null))
                {
                    erroringFields.Add(new NotNullViolation(notNullField, sourceMB));
                }
            }

            // Remove NotNullViolations for prefabs with IgnorePrefab

            bool isObjectAPrefab = PrefabUtility.GetPrefabAssetType(sourceMB.gameObject) != PrefabAssetType.NotAPrefab;
            List<NotNullViolation> violationsToIgnore = new List<NotNullViolation>();
            if (isObjectAPrefab)
            {
                // Find all violations that should be overlooked.
                foreach (NotNullViolation errorField in erroringFields)
                {
                    FieldInfo fieldInfo = errorField.FieldInfo;
                    foreach (Attribute attribute in Attribute.GetCustomAttributes(fieldInfo))
                    {
                        if (attribute.GetType() == typeof(NotNullAttribute))
                        {
                            if (((NotNullAttribute)attribute).IgnorePrefab)
                            {
                                violationsToIgnore.Add(errorField);
                            }
                        }
                    }
                }

                foreach (NotNullViolation violation in violationsToIgnore)
                {
                    erroringFields.Remove(violation);
                }
            }

            return erroringFields;
        }

        private static bool MonoBehaviourHasErrors(MonoBehaviour mb)
        {
            return FindErroringFields(mb).Count > 0;
        }
    }

    /// <summary>
    /// NotNullFinder fires off checks for NotNull violations in the scene and asset database
    /// and reports their errors.
    /// </summary>
    public class NotNullFinder : EditorWindow
    {
        private static readonly bool outputLogs = false;

        /// <summary>
        /// Searchs for and error for not null violations in the scene and asset database
        /// </summary>
        [MenuItem("Assets/Not Null Finder")]
        public static void SearchForAndErrorForNotNullViolations()
        {
            // Debug.Log ("Searching for null NotNull fields");
            // Search for and error for prefabs with null RequireWire fields
            string[] guidsForAllGameObjects = AssetDatabase.FindAssets("t:GameObject");
            foreach (string guid in guidsForAllGameObjects)
            {
                Log("Loading GUID: " + guid);
                string pathToGameObject = AssetDatabase.GUIDToAssetPath(guid);


                Log("Loading Asset for guid at path: " + pathToGameObject);
                GameObject gameObject = (GameObject)AssetDatabase.LoadAssetAtPath(pathToGameObject, typeof(GameObject));

                ErrorForNullRequiredWiresOnGameObject(gameObject, pathToGameObject);
            }

            // Search the scene objects (only need root game objects since children will be searched)
            GameObject[] sceneGameObjects = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
            List<GameObject> rootSceneGameObjects = new List<GameObject>();
            foreach (GameObject sceneGameObject in sceneGameObjects)
            {
                if (sceneGameObject.transform.parent == null)
                {
                    rootSceneGameObjects.Add(sceneGameObject);
                }
            }

            bool foundErrors = false;

            foreach (GameObject rootGameObjectInScene in rootSceneGameObjects)
            {
                foundErrors = foundErrors || ErrorForNullRequiredWiresOnGameObject(rootGameObjectInScene, "In current scene.");
            }

            if (foundErrors)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }

            // Debug.Log ("NotNull search complete");
        }

        private static bool ErrorForNullRequiredWiresOnGameObject(GameObject gameObject, string pathToAsset)
        {
            bool foundErrors = false;

            List<NotNullViolation> errorsOnGameObject = NotNullChecker.FindErroringFields(gameObject);
            foreach (NotNullViolation violation in errorsOnGameObject)
            {
                Debug.LogError(violation + "\nPath: " + pathToAsset, violation.ErrorGameObject);
                foundErrors = true;
            }

            foreach (Transform child in gameObject.transform)
            {
                foundErrors = foundErrors || ErrorForNullRequiredWiresOnGameObject(child.gameObject, pathToAsset);
            }

            return foundErrors;
        }

        private static void Log(string log)
        {
            if (outputLogs == false)
            {
                return;
            }

            Debug.Log(log);
        }
    }

    /// <summary>
    /// Drawerer for fields with NotNullAttribute assigned.
    /// </summary>
    [CustomPropertyDrawer(typeof(NotNullAttribute))]
    public class NotNullAttributeDrawer : PropertyDrawer
    {
        private readonly int warningHeight = 30;

        /// <summary>
        /// Gets the height that is passed into the rect in OnGUI.
        /// </summary>
        /// <returns>The property height.</returns>
        /// <param name="property">Serialized property.</param>
        /// <param name="label">Label for the property.</param>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // The height for the object assignment is just whatever Unity would
            // do by default.
            float objectReferenceHeight = base.GetPropertyHeight(property, label);
            float calculatedHeight = objectReferenceHeight;

            bool shouldAddWarningHeight = property.propertyType != SerializedPropertyType.ObjectReference ||
                                          IsNotWiredUp(property);
            if (shouldAddWarningHeight)
            {
                // When it's not wired up we add in additional height for the warning text.
                calculatedHeight += warningHeight;
            }

            return calculatedHeight;
        }

        /// <summary>
        /// Raises the GUI event and draws the property.
        /// </summary>
        /// <param name="position">Position for the property.</param>
        /// <param name="property">Serialized property.</param>
        /// <param name="label">Label for the property.</param>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Position is the DrawArea of the property to be rendered. Includes height from GetHeight()
            // BeginProperty used for objects that don't handle [SerializeProperty] attribute.
            EditorGUI.BeginProperty(position, label, property);

            // Calculate ObjectReference rect size
            Rect objectReferenceRect = position;

            // Use Unity's default height calculation for the reference rectangle
            float objectReferenceHeight = base.GetPropertyHeight(property, label);
            objectReferenceRect.height = objectReferenceHeight;
            BuildObjectField(objectReferenceRect, property, label);

            // Calculate warning rectangle's size
            Rect warningRect = new Rect(
                                   position.x,
                                   objectReferenceRect.y + objectReferenceHeight,
                                   position.width,
                                   warningHeight);
            BuildWarningRectangle(warningRect, property);

            EditorGUI.EndProperty();
        }

        private bool IsNotWiredUp(SerializedProperty property)
        {
            if (IsPropertyNotNullInSceneAndPrefab(property))
            {
                return false;
            }
            else
            {
                return property.objectReferenceValue == null;
            }
        }

        private bool IsPropertyNotNullInSceneAndPrefab(SerializedProperty property)
        {
            NotNullAttribute myAttribute = (NotNullAttribute)attribute;
            bool isPrefabAllowedNull = myAttribute.IgnorePrefab;
            return IsPropertyOnPrefab(property) && isPrefabAllowedNull;
        }

        private bool IsPropertyOnPrefab(SerializedProperty property)
        {
            return EditorUtility.IsPersistent(property.serializedObject.targetObject);
        }

        private void BuildObjectField(Rect drawArea, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.PropertyField(drawArea, property, label);
                return;
            }

            if (IsPropertyNotNullInSceneAndPrefab(property))
            {
                // Render Object Field for NotNull (InScene) attributes on Prefabs.
                label.text = "(*) " + label.text;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.ObjectField(drawArea, property, label);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                label.text = "* " + label.text;
                EditorGUI.ObjectField(drawArea, property, label);
            }
        }

        private void BuildWarningRectangle(Rect drawArea, SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                string warningString = "NotNullAttribute only valid on ObjectReference fields.";
                EditorGUI.HelpBox(drawArea, warningString, MessageType.Warning);
            }
            else if (IsNotWiredUp(property))
            {
                string warningString = "Missing object reference for NotNull property.";
                EditorGUI.HelpBox(drawArea, warningString, MessageType.Error);
            }
        }
    }

    /// <summary>
    /// Class that contains logic to find NotNull violations when pressing Play from the Editor
    /// </summary>
    [InitializeOnLoad]
    public class FindNotNullsOnLaunch
    {
        static FindNotNullsOnLaunch()
        {
            if (Debug.isDebugBuild)
            {
                // Searching on first launch seemed to execute before references were wired up on scene objects.
                EditorApplication.update += RunOnce;
            }
        }

        private static void RunOnce()
        {
            EditorApplication.update -= RunOnce;
            NotNullFinder.SearchForAndErrorForNotNullViolations();
        }
    }
#endif
}
