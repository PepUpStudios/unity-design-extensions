using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(ConditionHelpBoxAttribute))]
public class ConditionHelpBoxEditor : PropertyDrawer {

    object[] obj = new object[1];

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        position.y -= 10;
        position.height -= 10;
        label = EditorGUI.BeginProperty(position, label, property);
        ConditionHelpBoxAttribute attrb = attribute as ConditionHelpBoxAttribute;
        Object btnObj = Selection.activeGameObject.GetComponent(property.serializedObject.targetObject.GetType()) as Object;
        MethodInfo tMethod = btnObj.GetType().GetMethod(attrb.methodName);
        if (tMethod != null) {
            bool flag;
            if (attrb.parameter != null) {
                FieldInfo field = btnObj.GetType().GetField(attrb.parameter);
                obj[0] = field.GetValue(property.serializedObject.targetObject);
                flag = (bool)tMethod.Invoke(btnObj, obj);
            } else {
                flag = (bool)tMethod.Invoke(btnObj, null);
            }
            if (flag == attrb.activeOn) {
                EditorGUILayout.HelpBox(attrb.errorMessage, MessageType.Warning);
            } 
        } else {
            Debug.LogError("Method name not found!");
        }
        EditorGUI.EndProperty();
    }
}