using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DropDownListAttribute))]
public class DropDownListDrawer : PropertyDrawer {

    protected int prevUserIndex = 0;
    List<string> defaultKeys = new List<string>();

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        DropDownListAttribute attrib = attribute as DropDownListAttribute;

        string[] list = GetList(property, attrib);

        if (list == null || list.Length == 0 || (property.propertyType != SerializedPropertyType.String && property.propertyType != SerializedPropertyType.Integer &&
            property.propertyType != SerializedPropertyType.Float)) {
            return base.GetPropertyHeight(property, label) + 25f;
        }
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        if (property == null) {
            return;
        }

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        DropDownListAttribute attrib = attribute as DropDownListAttribute;

        string[] list = GetList(property, attrib);

        if (list == null) {
            EditorGUI.HelpBox(position, "Target Game Object is null, please make sure the target Game Object is active in hierarchy", MessageType.Warning);
            return;
        }

        if (list.Length == 0) {
            EditorGUI.HelpBox(position, "The target list is empty, cannot show value(s) on this element. Please add a value on the target list", MessageType.Warning);

            if (property.propertyType == SerializedPropertyType.String) {
                property.stringValue = null;
            } else if (property.propertyType == SerializedPropertyType.Integer) {
                property.intValue = ErrorKeyCode.EmptyList.GetHashCode();
            } else if (property.propertyType == SerializedPropertyType.Float) {
                property.floatValue = ErrorKeyCode.EmptyList.GetHashCode();
            }
            return;
        }

        if (property.propertyType == SerializedPropertyType.String) {
            string name = property.stringValue;
            DropDownList(position, list, ref name, attrib.indexValue);
            property.stringValue = name;
        } else if (property.propertyType == SerializedPropertyType.Integer) {
            string name = property.intValue.ToString();
            DropDownList(position, list, ref name, attrib.indexValue);
            name = GetDefaultKeyNumberValue(name);
            property.intValue = int.Parse(name);
        } else if (property.propertyType == SerializedPropertyType.Float) {
            string name = property.floatValue.ToString();
            DropDownList(position, list, ref name, attrib.indexValue);
            name = GetDefaultKeyNumberValue(name);
            property.floatValue = float.Parse(name);
        } else {
            EditorGUI.HelpBox(position, "DropDownList attribute only works with type string, int and float.", MessageType.Error);
        }

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    string[] GetList(SerializedProperty property, DropDownListAttribute attrib) {
        string[] list = new string[0];

        for (int i = 0; i < attrib.gameObjectTag.Length; i++) {
            string gameObjectTag = attrib.gameObjectTag[i];
            string componentName = attrib.componentName[i];
            string propertyName = attrib.propertyName[i];
            string propertyRelativeName = attrib.propertyRelativeName[i];
            string[] defaultKeys = attrib.defaultKeys;
            string prefix = attrib.prefix[i];
            SerializedObject serializedObj = GetSerializedObject(property, gameObjectTag, componentName);
            string[] temp = GetList(property, serializedObj, propertyName, propertyRelativeName, defaultKeys, prefix);
            list = MergeTwoArrays(list, temp);
        }
        return list;
    }

    SerializedObject GetSerializedObject(SerializedProperty mainProperty, string gameObjectTag, string componentName) {
        //find target list
        SerializedObject serializedObject = null;

        if (gameObjectTag.Equals("this") || componentName.Equals("this")) {
            Object obj = null;
            if (gameObjectTag.Equals("this")) {
                obj = mainProperty.serializedObject.targetObject;
            }
            if (componentName.Equals("this")) {
                serializedObject = mainProperty.serializedObject;
            } else {
                serializedObject = new SerializedObject(Selection.activeGameObject.GetComponent(componentName));
            }
        } else {
            GameObject gameObj = GameObject.FindGameObjectWithTag(gameObjectTag);
            if (gameObj != null) {
                Object obj = gameObj.GetComponent(componentName);
                if (obj != null) {
                    serializedObject = new SerializedObject(obj);
                }
            }
        }
        return serializedObject;
    }

    string[] GetList(SerializedProperty mainProperty, SerializedObject serializedObject, string propertyName, string propertyRelativeName, string[] defaultKeys, string prefix) {
        if (serializedObject == null) {
            return null;
        }

        List<string> temp = new List<string>();

        SerializedProperty property = serializedObject.FindProperty(propertyName);

        //load default keys
        this.defaultKeys.Clear();
        if (defaultKeys != null) {
            foreach (string key in defaultKeys) {
                this.defaultKeys.Add(key);
                temp.Add(key);
            }
        }

        //create list
        int length = property.arraySize;

        for (int i = 0; i < length; i++) {
            string name = null;
            string prefixTag = prefix;
            if (propertyRelativeName == null || propertyRelativeName.Equals("")) { //find value for premitive types
                if (property.arrayElementType.Equals("string")) {
                    name = property.GetArrayElementAtIndex(i).stringValue;
                } else if (property.arrayElementType.Equals("int")) {
                    name = property.GetArrayElementAtIndex(i).intValue.ToString();
                } else if (property.arrayElementType.Equals("float")) {
                    name = property.GetArrayElementAtIndex(i).floatValue.ToString();
                } else if (property.propertyType == SerializedPropertyType.Generic) {
                    name = i.ToString();
                }
            } else { //find value for generic types
                SerializedProperty propertyRelative = property.GetArrayElementAtIndex(i).FindPropertyRelative(propertyRelativeName);
                if (propertyRelative.propertyType == SerializedPropertyType.String) {
                    name = propertyRelative.stringValue;
                } else if (propertyRelative.propertyType == SerializedPropertyType.Integer) {
                    name = propertyRelative.intValue.ToString();
                } else if (propertyRelative.propertyType == SerializedPropertyType.Float) {
                    name = propertyRelative.floatValue.ToString();
                } else if (propertyRelative.propertyType == SerializedPropertyType.Generic) {
                    name = i.ToString();
                }
            }

            //add to list
            if (name != null) {
                if (name.Equals("")) {
                    temp.Add("Element " + i);
                } else if (prefixTag.Contains("$")) {
                    string[] split = prefixTag.Split('$');
                    temp.Add((GetPrefexKey(split[0], i) + split[1] + name));
                } else {
                    temp.Add((prefix + name));
                }
            }
        }
        return temp.ToArray();
    }

    string GetPrefexKey(string key, int counter) {
        if (key == "INDEX") {
            return counter.ToString();
        }
        return "";
    }

    void DropDownList(Rect position, string[] list, ref string name, bool indexValue) {
        SetPrevUserIndexFromProperty(name, list, indexValue);
        int userIndex = EditorGUI.Popup(position, prevUserIndex, list);
        name = (indexValue) ? userIndex.ToString() : list[userIndex];
    }

    void SetPrevUserIndexFromProperty(string name, string[] list, bool indexValue) {
        int tempCheck;
        for (int i = 0; i < list.Length; i++) {
            if (list[i] == null) continue;
            int iTemp = i;
            if (list[i].Equals(name) || (indexValue && i == int.Parse(name))) {
                prevUserIndex = i;
                break;
            } else if (int.TryParse(name, out tempCheck) && CheckForDefaultKeyValue(tempCheck, ref iTemp)) {
                prevUserIndex = iTemp;
                break;
            }
        }
    }

    string GetDefaultKeyNumberValue(string name) {
        for (int i = 0; i < defaultKeys.Count; i++) {
            if (defaultKeys[i] == null) continue;
            if (name.Contains(defaultKeys[i])) {
                int number = ((i - 1) < 0) ? (i - 1) : -(i + 1);//converts positive number in an array of default keys to neagtive number starting from -1
                return number.ToString();
            }
        }
        return name;
    }

    bool CheckForDefaultKeyValue(int defaultKeyValue, ref int iCounter) {
        if (defaultKeyValue < 0) { // default keys are mapped with negative numbers
            int number = (((defaultKeyValue + 1) >= 0) ? (defaultKeyValue + 1) : -(defaultKeyValue + 1));//converts negative default key number to positive number starting from 0
            for (int i = 0; i < defaultKeys.Count; i++) {
                if (number < defaultKeys.Count && number >= 0 && iCounter == number) {
                    iCounter = number;
                    return true;
                }
            }
        }
        return false;
    }

    T[] MergeTwoArrays<T>(T[] array1, T[] array2) {
        T[] newArray = new T[array1.Length + array2.Length];
        System.Array.Copy(array1, newArray, array1.Length);
        System.Array.Copy(array2, 0, newArray, array1.Length, array2.Length);
        return newArray;
    }
}