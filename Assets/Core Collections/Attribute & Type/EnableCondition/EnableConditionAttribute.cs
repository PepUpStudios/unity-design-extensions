using UnityEngine;

public class EnableConditionAttribute : PropertyAttribute {

    public string conditionVariableName;
    public string conditionValue;
    public int editorLength = 0;

    public EnableConditionAttribute(string conditionVariableName, string conditionValue) {
        this.conditionVariableName = conditionVariableName;
        this.conditionValue = conditionValue;
    }

    public EnableConditionAttribute(string conditionVariableName, string conditionValue, int editorLength) {
        this.conditionVariableName = conditionVariableName;
        this.conditionValue = conditionValue;
        this.editorLength = editorLength;
    }
}
