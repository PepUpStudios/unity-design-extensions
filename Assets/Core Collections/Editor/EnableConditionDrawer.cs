using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnableConditionAttribute))]
public class EnableConditionDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (!GetCondition(property)) {
            return base.GetPropertyHeight(property, label) - 18f;
        }
        EnableConditionAttribute attrib = attribute as EnableConditionAttribute;
        return base.GetPropertyHeight(property, label) + attrib.editorLength;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);

        if (GetCondition(property)) {
            EditorGUI.PropertyField(position, property, true);
        }            
        EditorGUI.EndProperty();
    }

    void SetLabelPosition(Rect position, GUIContent label) {
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
    }

    bool GetCondition(SerializedProperty property) {
        EnableConditionAttribute attrib = attribute as EnableConditionAttribute;

        string propertyPath = property.propertyPath;
        string conditionPath = attrib.conditionVariableName;
        string keyToFind = ".";
        if (propertyPath.Contains(keyToFind)) {
            int i = propertyPath.LastIndexOf(keyToFind);
            conditionPath = i < 0 ? "" : propertyPath.Substring(0, i + keyToFind.Length) + attrib.conditionVariableName;
        }
        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionPath);

        if (conditionProperty.propertyType == SerializedPropertyType.Boolean) {
            if (conditionProperty.boolValue.ToString().ToLower().Equals(attrib.conditionValue.ToLower())) {
                return true;
            }
        } else if (conditionProperty.propertyType == SerializedPropertyType.Integer) {
            if (conditionProperty.intValue.ToString().Equals(attrib.conditionValue)) {
                return true;
            }
        } else if (conditionProperty.propertyType == SerializedPropertyType.Float) {
            if (conditionProperty.floatValue.ToString().Equals(attrib.conditionValue)) {
                return true;
            }
        } else if (conditionProperty.propertyType == SerializedPropertyType.String) {
            if (conditionProperty.stringValue.Equals(attrib.conditionValue)) {
                return true;
            }
        } else if (conditionProperty.propertyType == SerializedPropertyType.Enum) {
            int value;
            if (int.TryParse(attrib.conditionValue, out value)) { //If value passed is an integer
                if (conditionProperty.enumValueIndex.Equals(value)) {
                    return true;
                }
            } else { //if value passed is string
                string[] names = conditionProperty.enumNames;
                if (names[conditionProperty.enumValueIndex].Equals(attrib.conditionValue)) {
                    return true;
                }
            }
        }
        return false;
    }
}