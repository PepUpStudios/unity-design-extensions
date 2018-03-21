using UnityEditor;
using UnityEditorInternal;
using State.Condition;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(MapStateCondition))]
public class MapStateConditionEditor : Editor {

    protected int prevUserIndex = 0;

    Rect Rect1;
    Rect Rect2;
    Rect Rect3;
    Rect Rect4;

    ReorderableList mapProfiles;
    ReorderableList powerUps;
    ReorderableList scores;

    MapStateCondition m_target;

    bool mapProfilesfoldout = false;
    bool powerUpsfoldout = false;
    bool scoresfoldout = false;

    void OnEnable() {
        m_target = target as MapStateCondition;

        SetUpMapProfile();
        SetUpPowerUps();
        SetUpScores();
    }

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        serializedObject.Update();

        mapProfilesfoldout = EditorGUILayout.Foldout(mapProfilesfoldout, "Profiles");
        if (mapProfilesfoldout) {
            mapProfiles.DoLayoutList();
            EditorGUILayout.Space();
        }
        powerUpsfoldout = EditorGUILayout.Foldout(powerUpsfoldout, "Powers");
        if (powerUpsfoldout) {
            powerUps.DoLayoutList();
            EditorGUILayout.Space();
        }
        scoresfoldout = EditorGUILayout.Foldout(scoresfoldout, "Scores");
        if (scoresfoldout) {
            scores.DoLayoutList();
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetUpPowerUps() {
        powerUps = new ReorderableList(serializedObject,
                serializedObject.FindProperty("powerUps"),
                true, true, true, true);

        powerUps.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var property = powerUps.serializedProperty.GetArrayElementAtIndex(index);

            var profileIndex = property.FindPropertyRelative("profileIndex");

            rect.y += 2;

            SetUpRects(rect, 0, 0, -(rect.height - 15f)); // 15 height of default property

            EditorGUI.PropertyField(Rect1, property.FindPropertyRelative("trigger"));

            Rect profileRect = Rect2;

            if (!m_target.gameObject.activeInHierarchy) {
                profileRect.height += 27f;
            } else if (Error.CheckErrorCode(profileIndex.intValue)) {
                profileRect.height += 37f;
            }

            EditorGUI.PropertyField(profileRect, profileIndex);

            //if (index != mapProfiles.serializedProperty.arraySize - 1) {
            //    string dash = "-";
            //    for (int i = 0; i < 1000; i++) {
            //        dash += '-';
            //    }
            //    EditorGUI.LabelField(lineRect, dash);
            //}
        };

        powerUps.elementHeightCallback = (index) => {
            Repaint();
            var property = powerUps.serializedProperty.GetArrayElementAtIndex(index);
            var profileIndex = property.FindPropertyRelative("profileIndex");

            float height = 48f;
            if (!m_target.gameObject.activeInHierarchy) {
                return height + 23f;
            } else if (Error.CheckErrorCode(profileIndex.intValue)) {
                height += 30f;
            }
            return height;
        };

        powerUps.drawHeaderCallback = rect => {
            EditorGUI.LabelField(rect, "Power Ups Mapping");
        };
    }

    void SetUpMapProfile() {
        mapProfiles = new ReorderableList(serializedObject,
                serializedObject.FindProperty("mapProfiles"),
                true, false, true, true);

        mapProfiles.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var property = mapProfiles.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            var description = property.FindPropertyRelative("description");
            var foldout = property.FindPropertyRelative("foldout");

            SetUpRects(rect, 0, 0, -(rect.height - 15f)); // 15 height of default property

            Rect1.x += 8;
            foldout.boolValue = EditorGUI.Foldout(Rect1, foldout.boolValue, description.stringValue);

            if (foldout.boolValue) {
                var outputType = property.FindPropertyRelative("outputType");
                var gameLevel = property.FindPropertyRelative("gameLevel");
                var enemyLevel = property.FindPropertyRelative("enemyLevel");

                EditorGUI.PropertyField(Rect2, description);
                EditorGUI.PropertyField(Rect3, outputType);

                if (outputType.enumValueIndex == OutputType.State.GetHashCode()) {
                    EditorGUI.PropertyField(Rect4, property.FindPropertyRelative("transitionState"));
                } else if (outputType.enumValueIndex == OutputType.GameLevel.GetHashCode()) {
                    Rect levelRect = this.Rect4;
                    if (!m_target.gameObject.activeInHierarchy) {
                        levelRect.height += 27f;
                    } else if (Error.CheckErrorCode(gameLevel.intValue)) {
                        levelRect.height += 37f;
                    }
                    EditorGUI.PropertyField(levelRect, gameLevel);
                } else if (outputType.enumValueIndex == OutputType.EnemyLevel.GetHashCode()) {
                    Rect levelRect = this.Rect4;
                    if (!m_target.gameObject.activeInHierarchy) {
                        levelRect.height += 27f;
                    } else if (Error.CheckErrorCode(enemyLevel.intValue)) {
                        levelRect.height += 37f;
                    }
                    EditorGUI.PropertyField(levelRect, enemyLevel);
                } else if (outputType.enumValueIndex == OutputType.Enemy.GetHashCode()) {
                    EditorGUI.PropertyField(Rect4, property.FindPropertyRelative("enemy"));
                }
            }
        };

        mapProfiles.elementHeightCallback = (index) => {
            Repaint();
            var property = mapProfiles.serializedProperty.GetArrayElementAtIndex(index);
            var outputType = property.FindPropertyRelative("outputType");
            var gameLevel = property.FindPropertyRelative("gameLevel");
            var enemyLevel = property.FindPropertyRelative("enemyLevel");
            var foldout = property.FindPropertyRelative("foldout");

            float height = (foldout.boolValue) ? 80f : 19f;
            if ((outputType.enumValueIndex == OutputType.GameLevel.GetHashCode() || outputType.enumValueIndex == OutputType.EnemyLevel.GetHashCode()) &&
            !m_target.gameObject.activeInHierarchy) {
                return height + 23f;
            } else if (Error.CheckErrorCode(gameLevel.intValue) || Error.CheckErrorCode(enemyLevel.intValue)) {
                height += 30f;
            }
            return height;
        };
    }

    void SetUpScores() {
        scores = new ReorderableList(serializedObject,
                serializedObject.FindProperty("scores"),
                true, false, true, true);

        scores.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
            var property = scores.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            var mapProfile = serializedObject.FindProperty("mapProfiles");
            var scoreType = property.FindPropertyRelative("scoreType");
            var profileIndex = property.FindPropertyRelative("profileIndex");
            var foldout = property.FindPropertyRelative("foldout");

            SetUpRects(rect, 0, 0, -(rect.height - 15f)); // 15 height of default property

            Rect1.x += 8;            
            string content = "Type: " + scoreType.enumDisplayNames[scoreType.enumValueIndex] + ", Profile: " + mapProfile.GetArrayElementAtIndex(profileIndex.intValue).FindPropertyRelative("description").stringValue;
            foldout.boolValue = EditorGUI.Foldout(Rect1, foldout.boolValue, content);

            if (foldout.boolValue) {
                var strategyType = property.FindPropertyRelative("strategy");
                var condition = property.FindPropertyRelative("condition");

                EditorGUI.PropertyField(Rect2, scoreType);
                EditorGUI.PropertyField(Rect3, strategyType);

                GUIContent markedValueTimer = new GUIContent();

                if (scoreType.enumValueIndex == ScoreType.Timer.GetHashCode()) {
                    markedValueTimer.text = "Score";
                } else {
                    markedValueTimer.text = "Kill Count";
                }

                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + 58f, rect.width, rect.height + -(rect.height - 32f))
                    , property.FindPropertyRelative("markedValue"), markedValueTimer);

                if (strategyType.enumValueIndex == StrategyType.Range.GetHashCode()) {
                    string[] names = condition.enumNames;
                    List<string> nameList = new List<string>();

                    SetPrevUserIndexFromProperty(names[condition.enumValueIndex], names);

                    //Convert prevUserIndex to match with names List index.
                    int j = 0;
                    foreach (string name in names) {
                        string tempName = name;
                        if (!name.Equals(ConditionType.GreaterThan.ToString()) && !name.Equals(ConditionType.LessThan.ToString())) {
                            int i = 0;
                            foreach (char ch in name.ToCharArray()) {
                                if (i != 0 && char.IsUpper(ch)) {
                                    tempName = tempName.Insert(i, " ");
                                    i++;
                                }
                                i++;
                            }
                            nameList.Add(tempName);
                            if (prevUserIndex == j) {
                                prevUserIndex = nameList.Count - 1;
                            }
                        }
                        j++;
                    }
                    if (prevUserIndex >= nameList.Count) {
                        prevUserIndex = 0;
                    }
                    int userIndex = EditorGUI.Popup(
                        new Rect(rect.x, rect.y + 96f, rect.width, rect.height + -(rect.height - 15f)),
                        "Condition", prevUserIndex, nameList.ToArray());

                    //Removes Space from nameList
                    nameList[userIndex] = nameList[userIndex].Replace(" ", "");

                    //Convert userIndex to match with names list index.
                    for (int i = 0; i < names.Length; i++) {
                        if (nameList[userIndex] == names[i]) {
                            userIndex = i;
                            break;
                        }
                    }

                    condition.enumValueIndex = userIndex;
                } else {
                    EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y + 96f, rect.width, rect.height + -(rect.height - 15f)),
                    condition);
                    var randomValue = property.FindPropertyRelative("randomValue");
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 132f, rect.width, rect.height + -(rect.height - 15f)), randomValue);

                    EditorGUI.HelpBox(new Rect(rect.x, rect.y + 150f, rect.width, rect.height + -(rect.height - 40f)),
                        "Random Range Select: Selects a random number from the range specified.", MessageType.Info);
                }

                if (m_target.gameObject.activeInHierarchy) {
                    if (Error.CheckErrorCode(profileIndex.intValue)) {
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 114f, rect.width, rect.height + -(rect.height - 15f) + 37), profileIndex);
                    } else {
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y + 114f, rect.width, rect.height + -(rect.height - 15f)), profileIndex);
                    }
                } else {
                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + 114f, rect.width, rect.height + -(rect.height - 15f) + 27), profileIndex);
                }
            }
        };

        scores.elementHeightCallback = (index) => {
            Repaint();
            var property = scores.serializedProperty.GetArrayElementAtIndex(index);
            var strategyType = property.FindPropertyRelative("strategy");
            var profileIndex = property.FindPropertyRelative("profileIndex");
            var foldout = property.FindPropertyRelative("foldout");
            
            float height = (foldout.boolValue) ? 144f : 19f;

            if (!m_target.gameObject.activeInHierarchy) {
                height += 23f;
            } else if (Error.CheckErrorCode(profileIndex.intValue)) {
                height += 30f;
            }

            if (foldout.boolValue && strategyType.enumValueIndex == StrategyType.RandomRangeSelect.GetHashCode()) {
                height += 62f;
            }
            return height;
        };
    }

    void SetPrevUserIndexFromProperty(string name, string[] list) {
        for (int i = 0; i < list.Length; i++) {
            if (list[i].Equals(name)) {
                prevUserIndex = i;
                break;
            }
        }
    }

    void SetUpRects(Rect position, float leftIndent, float width, float height) {
        Rect1 = new Rect(position.x + leftIndent, position.y, position.width + width, position.height + height);
        Rect2 = new Rect(position.x + leftIndent, position.y + 18, position.width + width, position.height + height);
        Rect3 = new Rect(position.x + leftIndent, position.y + 36, position.width + width, position.height + height);
        Rect4 = new Rect(position.x + leftIndent, position.y + 54, position.width + width, position.height + height);
    }
}