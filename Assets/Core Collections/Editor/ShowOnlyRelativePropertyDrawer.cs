using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ShowOnlyRelativePropertyAttribute)), CanEditMultipleObjects]
public class ShowOnlyRelativePropertyDrawer : PropertyDrawer {

    protected int incrementFactor = 18;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        ShowOnlyRelativePropertyAttribute attrb = (ShowOnlyRelativePropertyAttribute)attribute;
        return attrb.relativeProperties.Length * incrementFactor;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        property.serializedObject.Update();
        label = EditorGUI.BeginProperty(position, label, property);

        ShowOnlyRelativePropertyAttribute attrb = (ShowOnlyRelativePropertyAttribute)attribute;        
        Rect pos = position;
        foreach (string relativeProperty in attrb.relativeProperties) {
            var relative = property.FindPropertyRelative(relativeProperty);
            EditorGUI.PropertyField(pos, relative);
            pos.y += incrementFactor;
        }
        EditorGUI.EndProperty();
        property.serializedObject.ApplyModifiedProperties();
    }
}