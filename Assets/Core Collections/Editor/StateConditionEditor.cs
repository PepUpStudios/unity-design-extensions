using UnityEditor;
using State.Condition;

[CustomEditor(typeof(CollisionStateCondition)), CanEditMultipleObjects]
public class StateConditionEditorEditor : Editor {

    bool show = false;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        show = EditorGUILayout.Foldout(show, "Definations:");
        if (show) {
            EditorGUILayout.HelpBox("State Profile:-\n- Contains the list of state to map height, orientation and override state profile.\n- OrderID must be unique for each state profile.\n\nHeight & Orientation:-\n- One to one mapping of height type and state profile.\n\nOverride State Profile:-\n- Can override for State and Obstacle.\n- Evaluated order index from height and orientation is first filtered by Obstacle then State.\n- Can also override order index mapping to a different transition name or/and land state.", MessageType.Info);
        }
    }
}
