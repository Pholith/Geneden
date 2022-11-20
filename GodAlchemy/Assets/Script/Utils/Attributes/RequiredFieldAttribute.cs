using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class RequiredFieldAttribute : PropertyAttribute
{
    public enum FieldColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }
    public Color color;

    public RequiredFieldAttribute(float r, float g = 0, float b = 0)
    {
        color = new Color(r, g, b, 1);
    }
    public RequiredFieldAttribute(FieldColor _color = FieldColor.Red)
    {
        switch (_color)
        {
            case FieldColor.Red:
                color = Color.red;
                break;
            case FieldColor.Green:
                color = Color.green;
                break;
            case FieldColor.Blue:
                color = Color.blue;
                break;
            case FieldColor.Yellow:
                color = Color.yellow;
                break;
            default:
                color = Color.red;
                break;
        }
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RequiredFieldAttribute))]
public class RequiredFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RequiredFieldAttribute field = attribute as RequiredFieldAttribute;

        if (property.propertyType == SerializedPropertyType.Generic)
        {
            Debug.LogError($"RequiredFieldAttribute is not available on type property of {property.type} ({property.name})");
            return;
        }
        
        if (property.objectReferenceValue == null)
        {
            GUI.color = field.color; //Set the color of the GUI
            EditorGUI.PropertyField(position, property, label); //Draw the GUI
            GUI.color = Color.white; //Reset the color of the GUI to white
            Debug.LogWarning($"Field {property.name} cannot be null !");
        }
        else
            EditorGUI.PropertyField(position, property, label);
    }
}
#endif