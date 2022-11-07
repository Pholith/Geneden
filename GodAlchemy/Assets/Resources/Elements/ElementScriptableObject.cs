using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New element", order = 1)]
public class ElementScriptableObject : ScriptableObject
{
    public string ElementDescription;

    [HideInInspector]
    public Sprite Sprite;

    [Range(1, 3)]
    public int DifficultyToCraft = 1;
    public bool IsPrimaryElement = false;

    [HideInInspector]
    public ElementScriptableObject ElementToCraft1;
    [HideInInspector]
    public ElementScriptableObject ElementToCraft2;

    [SerializeField]
    public UnityEvent EffectOnMap;

    public int GetCost()
    {
        return DifficultyToCraft switch
        {
            1 => 10,
            2 => 20,
            3 => 40,
            _ => throw new ArgumentOutOfRangeException("Argument must be between 1 and 3 included"),
        };
    }

    // a voir si c'est nécessaire
    /*public static bool operator ==(ElementScriptableObject element2)
    {
        return false;
    }*/
}

#if UNITY_EDITOR

[CustomEditor(typeof(ElementScriptableObject))]
public class ElementInspector : Editor
{
    private ElementScriptableObject targetElement;

    private SerializedProperty sprite;
    private SerializedProperty elementToCraft1;
    private SerializedProperty elementToCraft2;

    private readonly GUIContent elementGUIContent1 = new ("");
    private readonly GUIContent elementGUIContent2 = new ("");

    // Sert uniquement à afficher les menus de dropdown au bon endroit
    private Rect dropdownRect1 = Rect.zero;
    private Rect dropdownRect2 = Rect.zero;

    protected void OnEnable()
    {
        targetElement = target as ElementScriptableObject;
        sprite = serializedObject.FindProperty("Sprite");
        elementToCraft1 = serializedObject.FindProperty("ElementToCraft1");
        elementToCraft2 = serializedObject.FindProperty("ElementToCraft2");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawDefaultInspector();

        elementGUIContent1.text = elementToCraft1.objectReferenceValue?.name ?? "Element 1";
        elementGUIContent2.text = elementToCraft2.objectReferenceValue?.name ?? "Element 2";

        // Sprite
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Sprite");
        sprite.objectReferenceValue = EditorGUILayout.ObjectField(sprite.objectReferenceValue, typeof(Sprite), true, GUILayout.Height(64), GUILayout.Width(64)) as Sprite;

        EditorGUILayout.EndHorizontal();
        
        // Ajout d'une fusion
        if (!targetElement.IsPrimaryElement)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Is crafted from:", GUILayout.Width(100));

            bool dropdownClicked1 = EditorGUILayout.DropdownButton(elementGUIContent1, FocusType.Passive, GUILayout.ExpandWidth(false));
            if (Event.current.type == EventType.Repaint) dropdownRect1 = GUILayoutUtility.GetLastRect();
            if (dropdownClicked1)
            {
                GenericMenu dropdownMenu1 = new();
                foreach (ElementScriptableObject elementScriptableObj in UnityUtils.GetAllInstances<ElementScriptableObject>())
                {
                    dropdownMenu1.AddItem(new GUIContent(elementScriptableObj.name), false, OnElement1Selected, elementScriptableObj);
                }
                dropdownMenu1.DropDown(dropdownRect1);
            }

            EditorGUILayout.LabelField("+", GUILayout.Width(10));
            bool dropdownClicked2 = EditorGUILayout.DropdownButton(elementGUIContent2, FocusType.Passive, GUILayout.ExpandWidth(false));
            if (Event.current.type == EventType.Repaint) dropdownRect2 = GUILayoutUtility.GetLastRect();
            if (dropdownClicked2)
            {
                GenericMenu dropdownMenu = new();
                foreach (ElementScriptableObject elementScriptableObj in UnityUtils.GetAllInstances<ElementScriptableObject>())
                {
                    dropdownMenu.AddItem(new GUIContent(elementScriptableObj.name), false, OnElement2Selected, elementScriptableObj);
                }
                dropdownMenu.DropDown(dropdownRect2);
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnElement1Selected(object elementScriptableObj)
    {

        ElementScriptableObject element = elementScriptableObj as ElementScriptableObject;
        elementGUIContent1.text = element.name;
        elementToCraft1.objectReferenceValue = element;
        serializedObject.ApplyModifiedProperties();

    }
    private void OnElement2Selected(object elementScriptableObj)
    {
        ElementScriptableObject element = elementScriptableObj as ElementScriptableObject;
        elementGUIContent2.text = element.name;
        elementToCraft2.objectReferenceValue = element;
        serializedObject.ApplyModifiedProperties();

    }
}

#endif
