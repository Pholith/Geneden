using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HighlightAttribute : PropertyAttribute
{
    public Color col;
    public HighlightAttribute(float r = 1, float g = 0, float b = 0)
    {
        col = new Color(r, g, b, 1);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(HighlightAttribute))]
public class HighlightPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Color col = (attribute as HighlightAttribute).col;
        Color prev = GUI.color;
        GUI.color = col;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.color = prev;

    }
}
#endif