using UnityEngine;
using UnityEngine.Events;
using System;

#if UNITY_EDITOR
using UnityEditor;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/New element", order = 1)]
public class ElementScriptableObject : ScriptableObject
{
    public string ElementDescription;

    [HideInInspector, SerializeField]
    public Sprite Sprite;

    [Range(1, 3)]
    public int DifficultyToCraft = 1;
    public bool IsPrimaryElement = false;

    [HideInInspector, SerializeField]
    public ElementScriptableObject ElementToCraft1;
    [HideInInspector, SerializeField]
    public ElementScriptableObject ElementToCraft2;

    [SerializeField]
    public UnityEvent EffectOnMap;


    public int GetElementPrice()
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

[CustomEditor(typeof(ElementScriptableObject))]
public class ElementInspector : Editor
{
    private ElementScriptableObject targetElement;

    private readonly GUIContent elementGUIContent1 = new GUIContent("");
    private readonly GUIContent elementGUIContent2 = new GUIContent("");

    // Sert uniquement à afficher les menus de dropdown au bon endroit
    private Rect dropdownRect1 = Rect.zero;
    private Rect dropdownRect2 = Rect.zero;

    public override void OnInspectorGUI()
    {
        targetElement = target as ElementScriptableObject;

        elementGUIContent1.text = targetElement.ElementToCraft1?.name ?? "Element 1";
        elementGUIContent2.text = targetElement.ElementToCraft2?.name ?? "Element 2";
        DrawDefaultInspector();

        // Sprite
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Sprite");
        targetElement.Sprite = EditorGUILayout.ObjectField(targetElement.Sprite, typeof(Sprite), true,
        GUILayout.Height(64), GUILayout.Width(64)) as Sprite;
        Undo.RecordObject(targetElement, $"Ajout d'un sprite sur l'élément {targetElement.name}");
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
        }
    }

    private void OnElement1Selected(object elementScriptableObj)
    {
        ElementScriptableObject element = elementScriptableObj as ElementScriptableObject;
        elementGUIContent1.text = element.name;
        targetElement.ElementToCraft1 = element;

    }
    private void OnElement2Selected(object elementScriptableObj)
    {
        ElementScriptableObject element = elementScriptableObj as ElementScriptableObject;
        elementGUIContent2.text = element.name;
        targetElement.ElementToCraft2 = element;
    }
}

#endif
