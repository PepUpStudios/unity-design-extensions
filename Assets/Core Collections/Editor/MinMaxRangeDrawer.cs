using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
[CustomPropertyDrawer(typeof(RangedFloat))]
[CustomPropertyDrawer(typeof(RangedInt))]
[CustomPropertyDrawer(typeof(RangedUint))]
public class MinMaxRangeDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (property.serializedObject.isEditingMultipleObjects) {
            return 0f;
        } else if (property.FindPropertyRelative("minValue") == null) {
            return base.GetPropertyHeight(property, label);
        }
        return base.GetPropertyHeight(property, label) + 16f;
    }


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property.serializedObject.isEditingMultipleObjects) {
            return;
        }

        var minProperty = property.FindPropertyRelative("minValue");

        if (minProperty == null) {
            EditorGUI.LabelField(position, label.text, "MinMaxRange attribute only works with type RangedFloat or RangedInt.");
            Debug.LogError("MinMaxRange attribute only works with type RangedFloat or RangedInt.");
            return;
        }

        var maxProperty = property.FindPropertyRelative("maxValue");
        var minmax = attribute as MinMaxRangeAttribute ?? new MinMaxRangeAttribute(0, 1);
        position.height -= 16f;

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var left = new Rect(position.x, position.y, position.width / 2 - 11f, position.height);
        var right = new Rect(position.x + position.width - left.width, position.y, left.width, position.height);
        var mid = new Rect(left.xMax, position.y, 22, position.height);

        if (maxProperty.propertyType == SerializedPropertyType.Float) {
            float min = minProperty.floatValue;
            float max = maxProperty.floatValue;

            min = Mathf.Clamp(EditorGUI.FloatField(left, min), minmax.minLimit, max);
            EditorGUI.LabelField(mid, " to ");
            max = Mathf.Clamp(EditorGUI.FloatField(right, max), min, minmax.maxLimit);

            position.y += 16f;
            EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, minmax.minLimit, minmax.maxLimit);

            minProperty.floatValue = min;
            maxProperty.floatValue = max;
        } else if (maxProperty.propertyType == SerializedPropertyType.Integer) {
            float min = minProperty.intValue * 1.0f;
            float max = maxProperty.intValue * 1.0f;

            int minLimit = Mathf.RoundToInt(minmax.minLimit);
            int maxLimit = Mathf.RoundToInt(minmax.maxLimit);

            min = Mathf.Clamp(EditorGUI.IntField(left, Mathf.RoundToInt(min)), minLimit, max);
            EditorGUI.LabelField(mid, " to ");
            max = Mathf.Clamp(EditorGUI.IntField(right, Mathf.RoundToInt(max)), min, maxLimit);

            position.y += 16f;
            EditorGUI.MinMaxSlider(position, GUIContent.none, ref min, ref max, minmax.minLimit, minmax.maxLimit);

            minProperty.intValue = Mathf.RoundToInt(min);
            maxProperty.intValue = Mathf.RoundToInt(max);
        }

        EditorGUI.EndProperty();
    }
}