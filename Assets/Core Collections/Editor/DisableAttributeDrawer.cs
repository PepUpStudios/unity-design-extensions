using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DisableAttribute))]
public class DisableAttributeDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, true);
        GUI.enabled = true;
    }
}