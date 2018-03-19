using UnityEngine;

public class ConditionHelpBoxAttribute : PropertyAttribute {

    public string methodName;
    public string parameter;
    public string errorMessage;
    public bool activeOn;

    public ConditionHelpBoxAttribute(string methodName, string parameter, bool activeOn, string errorMessage) {
        this.methodName = methodName;
        this.parameter = parameter;
        this.activeOn = activeOn;
        this.errorMessage = errorMessage;
    }
}