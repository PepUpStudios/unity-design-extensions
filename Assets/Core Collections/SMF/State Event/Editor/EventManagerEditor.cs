using UnityEngine;
using UnityEditor;
using State.Event;
using UnityEditorInternal;
using System.Reflection;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(EventManager), true), CanEditMultipleObjects]
public class EventManagerEditor : Editor {

    SerializedProperty eventObjs;

    //Gets Reset OnEnable
    Dictionary<string, MyReorderableList> eventMap = new Dictionary<string, MyReorderableList>();
    List<string> activeNames = new List<string>();
    Dictionary<string, int> indexMap = new Dictionary<string, int>();

    //Gets Reset Every Frame
    Dictionary<int, string> sourceEventMap = new Dictionary<int, string>();
    Dictionary<int, UnityEngine.Object> sourceComponentMap = new Dictionary<int, UnityEngine.Object>();

    //For finding specific methods
    BindingFlags methodFlag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.ExactBinding; //Method finding filters
    Type[,] methodSignature = new Type[,] { { typeof(float[]) }, { typeof(object[]) } }; //Get signature methods;

    bool showEventObj = false;
    bool showEventObjRelative = true;

    private void OnEnable() {
        showEventObj = false;
        showEventObjRelative = true;
        InitProperties();
        //Resets temporary variables
        eventMap.Clear();
        activeNames.Clear();
        indexMap.Clear();
        //Gets Medthod By filter
        MethodInfo[] methodsInfo = GetSpecificParamMethods(methodSignature, methodFlag);

        InitNameAndMap(methodsInfo);
    }

    void InitNameAndMap(MethodInfo[] methodsInfo) {
        int length = methodsInfo.Length;
        int index = 0;
        for (int i = 0; i < length; i++) {
            MyReorderableList obj = new MyReorderableList();
            string name = methodsInfo[i].Name;
            obj.active = GetActiveStatus(name);
            List<EventVo> eventVoTemp = GetEventVoFromProperty(eventObjs, name);
            SetIndexMapping(eventVoTemp, name, index);
            ReorderableList myReorderableList = InitReorderable(eventVoTemp);
            if (obj.active) {
                activeNames.Add(name);
            }
            SetReorderableEvents(myReorderableList, eventObjs, eventVoTemp, name, index);
            obj.reorderableList = myReorderableList;
            eventMap.Add(name, obj);
            index++;
        }
    }

    List<EventVo> GetEventVoFromProperty(SerializedProperty property, string name) {
        List<EventVo> eventVo = new List<EventVo>();
        for (int i = 0; i < property.arraySize; i++) {
            string targetMethod = property.GetArrayElementAtIndex(i).FindPropertyRelative("targetMethod").stringValue;
            if (name == targetMethod) {
                EventVo temp = new EventVo();
                temp.sourceObj = (GameObject)property.GetArrayElementAtIndex(i).FindPropertyRelative("sourceObj").objectReferenceValue;
                temp.sourceComponent = property.GetArrayElementAtIndex(i).FindPropertyRelative("sourceComponent").objectReferenceValue;
                temp.sourceEvent = property.GetArrayElementAtIndex(i).FindPropertyRelative("sourceEvent").stringValue;
                temp.targetMethod = targetMethod;
                eventVo.Add(temp);
            }
        }
        return eventVo;
    }

    void SetIndexMapping(List<EventVo> eventVo, string name, int nameIndex) {
        EventManager myEventScript = (EventManager)target;
        List<EventVo> temp = (myEventScript.eventObjs == null) ? new List<EventVo>() : new List<EventVo>(myEventScript.eventObjs);
        int counter = 0;
        for (int i = 0; i < eventVo.Count; i++) {
            for (int j = 0; j < temp.Count; j++) {
                if (name == temp[j].targetMethod) {
                    string newIndexCode = nameIndex + "-" + i;
                    indexMap.Add(newIndexCode, j + counter);
                    temp.RemoveAt(j);
                    counter++;
                    break;
                }
            }
        }
    }

    bool GetActiveStatus(string name) {
        if (eventObjs == null) {
            return false;
        }
        for (int i = 0; i < eventObjs.arraySize; i++) {
            SerializedProperty targetMethod = eventObjs.GetArrayElementAtIndex(i).FindPropertyRelative("targetMethod");
            if (targetMethod == null) {
                return false;
            } else if (targetMethod.stringValue == name || targetMethod.stringValue == null) {
                return true;
            }
        }
        return false;
    }

    void InitProperties() {
        eventObjs = serializedObject.FindProperty("eventObjs");
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (eventMap.Count == 0) {
            return;
        }
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Event Methods", EditorStyles.boldLabel);

        if (activeNames.Count > 0) {
            DrawActiveReorderable(activeNames.ToArray());
        }

        //To make it in center
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Add New Event Method", GUILayout.Width(250))) {
            GenericMenu toolsMenu = new GenericMenu();
            foreach (string name in eventMap.Keys) {
                if (eventMap.ContainsKey(name) && eventMap[name].active) {
                    toolsMenu.AddDisabledItem(new GUIContent(name));
                } else {
                    toolsMenu.AddItem(new GUIContent(name), false, SetActiveReorderable, name);
                }
            }
            toolsMenu.ShowAsContext();
            Event.current.Use();
        }
        GUILayout.FlexibleSpace();
        GUIStyle style = new GUIStyle();
        style.margin = new RectOffset(0, 0, 3, 20);
        if (!showEventObj) {
            if (GUILayout.Button("↓", style, GUILayout.Width(10))) {
                showEventObj = true;
            }
        }
        if (showEventObj) {
            if (GUILayout.Button("↑", style, GUILayout.Width(10))) {
                showEventObj = false;
            }
        }
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (showEventObj) {
            showEventObjRelative = EditorGUILayout.Foldout(showEventObjRelative, "Internal Event Data");
            if (showEventObjRelative) DrawList(eventObjs);
        }
        serializedObject.ApplyModifiedProperties();
    }
    
    void DrawList(SerializedProperty thisList) {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;
        //Resize our list
        int ListSize = thisList.arraySize;
        ListSize = EditorGUILayout.IntField ("Size", ListSize);
   
        if(ListSize != thisList.arraySize){
            while(ListSize > thisList.arraySize){
                thisList.InsertArrayElementAtIndex(thisList.arraySize);
            }
            while(ListSize < thisList.arraySize){
                thisList.DeleteArrayElementAtIndex(thisList.arraySize - 1);
            }
        }
   
        //Display our list to the inspector window
        for(int i = 0; i < thisList.arraySize; i++){
            SerializedProperty MyListRef = thisList.GetArrayElementAtIndex(i);
            SerializedProperty sourceObj = MyListRef.FindPropertyRelative("sourceObj");
            SerializedProperty sourceComponent = MyListRef.FindPropertyRelative("sourceComponent");
            SerializedProperty sourceEvent = MyListRef.FindPropertyRelative("sourceEvent");
            SerializedProperty targetMethod = MyListRef.FindPropertyRelative("targetMethod");
            SerializedProperty isFold = MyListRef.FindPropertyRelative("isFold");
            
            EditorGUI.indentLevel = 1;
            isFold.boolValue = EditorGUILayout.Foldout(isFold.boolValue, "Event " + i);
            // Display the property fields in two ways.
            if (isFold.boolValue) {
                EditorGUI.indentLevel = 2;
                EditorGUILayout.PropertyField(sourceObj);
                EditorGUILayout.PropertyField(sourceComponent);
                EditorGUILayout.PropertyField(sourceEvent);
                EditorGUILayout.PropertyField(targetMethod);

                if(GUILayout.Button("Remove Event " + i)){
                    thisList.DeleteArrayElementAtIndex(i);
                }
            }
        }
        EditorGUI.indentLevel = indent;
    }

    void DrawActiveReorderable(string[] names) {
        foreach (string name in names) {
            eventMap[name].reorderableList.DoLayoutList();
            EditorGUILayout.Space();
        }
    }

    void SetActiveReorderable(object nameObj) {
        string name = (string)nameObj;
        if (eventMap.ContainsKey(name)) {
            MyReorderableList obj = eventMap[name];
            if (!obj.active) {
                obj.active = true;
                activeNames.Add(name);
            }
        }
    }

    ReorderableList InitReorderable(SerializedProperty mainProperty) {
        ReorderableList reorderable = new ReorderableList(serializedObject, mainProperty, false, true, true, true);
        return reorderable;
    }

    ReorderableList InitReorderable<T>(List<T> list) {
        ReorderableList reorderable = new ReorderableList(list, typeof(T), false, true, true, true);
        return reorderable;
    }

    void SetReorderableEvents(ReorderableList reorderable, SerializedProperty mainProperty, List<EventVo> thisEventVo, string name, int nameIndex) {
        EventManager myEventScript = (EventManager)target;
        GUIContent label = new GUIContent();

        reorderable.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, name);
            GUIStyle style = new GUIStyle();
            Rect buttonRect = new Rect(rect.width + 6, rect.y, rect.width, rect.height);
            if (GUI.Button(buttonRect, "x", style) && EditorUtility.DisplayDialog(" Are you sure?", "Do you want to remove " +
                "all the events from \"" + name + "\" event method? \n\n You cannot undo this action.", "Yes", "No")) {
                for (int i = 0; i < activeNames.Count; i++) {
                    if (activeNames[i].Equals(name)) {
                        activeNames.RemoveAt(i);
                        eventMap[name].active = false;
                    }
                }
                for (int i = 0; i < myEventScript.eventObjs.Count; i++) {
                    if (myEventScript.eventObjs[i].targetMethod == name) {
                        myEventScript.eventObjs.RemoveAt(i);
                        i--;
                    }
                }
                foreach (string meathodName in eventMap.Keys) {
                    if (meathodName == name) {
                        for (int i = 0; i < eventMap[name].reorderableList.list.Count; i++) {
                            eventMap[name].reorderableList.list.RemoveAt(i);
                            i--;
                        }
                        break;
                    }
                }
                ResetIndexMapping();
            }
        };

        reorderable.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            Repaint();
            Type property = reorderable.list[index].GetType();

            var sourceObj = property.GetField("sourceObj").GetValue(thisEventVo[index]);
            var sourceComponent = property.GetField("sourceComponent").GetValue(thisEventVo[index]);
            var sourceEvent = property.GetField("sourceEvent").GetValue(thisEventVo[index]);

            //Init Rect
            Rect eventPopUpRect = new Rect(rect.x, rect.y, rect.width / 2 - 10, rect.height);
            Rect sourceObjRect = new Rect(rect.x + rect.width / 2, rect.y, rect.width / 2, rect.height - 5);

            List<string> sourceList = new List<string>();
            GameObject sourceGameObject = (GameObject)sourceObj;
            if (sourceGameObject != null) {
                sourceList = GetEventNames(sourceGameObject, nameIndex);
            }
            int selectedIndex = 0;
            if (sourceComponent != null && sourceEvent != null) {
                selectedIndex = GetSelectedIndex(sourceList, sourceComponent.GetType().Name, sourceEvent.ToString());
            }

            if (sourceList.Count == 0) {
                GUI.enabled = false;
                sourceList.Add("No Events Found...");
                selectedIndex = EditorGUI.Popup(eventPopUpRect, selectedIndex, sourceList.ToArray());
                GUI.enabled = true;
                sourceComponentMap[selectedIndex] = null;
                sourceEventMap[selectedIndex] = null;
            } else {
                selectedIndex = EditorGUI.Popup(eventPopUpRect, selectedIndex, sourceList.ToArray());
            }

            int eventObjIndex = indexMap[nameIndex + "-" + index];
            SerializedProperty eventObj = mainProperty.GetArrayElementAtIndex(eventObjIndex);
            SerializedProperty gameObjectField = eventObj.FindPropertyRelative("sourceObj");
            EditorGUI.PropertyField(sourceObjRect, gameObjectField, label);

            //Save back to EventVO
            myEventScript.eventObjs[eventObjIndex].sourceObj = (GameObject)gameObjectField.objectReferenceValue;
            property.GetField("sourceObj").SetValue(thisEventVo[index], gameObjectField.objectReferenceValue);
            if (sourceComponentMap.ContainsKey(selectedIndex)) {
                myEventScript.eventObjs[eventObjIndex].sourceComponent = sourceComponentMap[selectedIndex];
                property.GetField("sourceComponent").SetValue(thisEventVo[index], sourceComponentMap[selectedIndex]);
                eventObj.FindPropertyRelative("sourceComponent").objectReferenceValue = sourceComponentMap[selectedIndex];
            }
            if (sourceEventMap.ContainsKey(selectedIndex)) {
                myEventScript.eventObjs[eventObjIndex].sourceEvent = sourceEventMap[selectedIndex];
                property.GetField("sourceEvent").SetValue(thisEventVo[index], sourceEventMap[selectedIndex]);
                eventObj.FindPropertyRelative("sourceEvent").stringValue = sourceEventMap[selectedIndex];
            }
            myEventScript.eventObjs[eventObjIndex].targetMethod = name;
            property.GetField("targetMethod").SetValue(thisEventVo[index], name);
            eventObj.FindPropertyRelative("targetMethod").stringValue = name;
        };

        reorderable.onAddCallback = (ReorderableList list) => {
            EventVo newEventVo = new EventVo();
            newEventVo.targetMethod = name;

            myEventScript.eventObjs.Add(newEventVo);
            list.list.Add(newEventVo);

            string newIndexCode = nameIndex + "-" + (list.list.Count - 1);
            indexMap.Add(newIndexCode, myEventScript.eventObjs.Count - 1);
        };

        reorderable.onRemoveCallback = (ReorderableList list) => {
            myEventScript.eventObjs.RemoveAt(indexMap[nameIndex + "-" + list.index]);
            list.list.RemoveAt(list.index);
            ResetIndexMapping();
        };
    }

    void ResetIndexMapping() {
        indexMap.Clear();
        int counter = 0;
        foreach (string name in eventMap.Keys) {
            List<EventVo> eventVo = eventMap[name].reorderableList.list as List<EventVo>;
            SetIndexMapping(eventVo, name, counter);
            counter++;
        }
    }

    int GetSelectedIndex(List<string> sourceList, string sourceComponentName, string sourceEventName) {
        for (int i = 0; i < sourceList.Count; i++) {
            if (sourceList[i].Contains(sourceComponentName + '/' + sourceEventName)) {
                return i;
            }
        }
        return 0;
    }

    List<string> GetEventNames(GameObject source, int nameIndex) {
        List<string> eventNames = new List<string>();
        Component[] eventComponentNames = GetEventComponent(source);
        int counter = 0;
        if (eventComponentNames.Length > 0) {
            sourceComponentMap.Clear();
            sourceEventMap.Clear();
            foreach (Component eventManager in eventComponentNames) {
                Type eventType = eventManager.GetType();
                string name = eventType.ToString();
                foreach (EventInfo eventInfo in GetEvents(eventType)) {
                    string eventName = name + '/' + eventInfo.Name;
                    eventNames.Add(eventName);
                    sourceComponentMap.Add(counter, eventManager);
                    sourceEventMap.Add(counter, eventInfo.Name);
                    counter++;
                }
            }
        }
        return eventNames;
    }

    Component[] GetEventComponent(GameObject source) {
        Component[] eventManagers = source.GetComponents<Component>();
        return eventManagers;
    }

    EventInfo[] GetEvents(Type type) {
        EventInfo[] eventsInfo = type.GetEvents();
        return eventsInfo;
    }

    MethodInfo[] GetMethods(BindingFlags flags) {
        EventManager myEventScript = (EventManager)target;
        MethodInfo[] methodsInfo = myEventScript.GetType().GetMethods(flags);
        return methodsInfo;
    }

    MethodInfo[] GetSpecificParamMethods(Type[,] types, BindingFlags flags) {
        List<MethodInfo> verifiedMethods = new List<MethodInfo>();
        MethodInfo[] methodsInfo = GetMethods(flags);
        foreach (MethodInfo methodInfo in methodsInfo) {
            bool flag = false;
            ParameterInfo[] parameters = methodInfo.GetParameters();
            //Check if methode is of right signature for event manager
            for (int i = 0; i < types.Length; i++) {
                if (parameters.Length == types.GetUpperBound(0)) {
                    for (int j = 0; j < parameters.Length; j++) {
                        flag = false;
                        if (parameters[j].ParameterType == types[i,j]) {
                            flag = true;
                        }
                    }
                    if (flag) {
                        verifiedMethods.Add(methodInfo);
                        break;
                    }
                }
            }
        }
        return verifiedMethods.ToArray();
    }

    [SerializeField]
    public class MyReorderableList {
        public ReorderableList reorderableList;
        public bool active;
    }
}