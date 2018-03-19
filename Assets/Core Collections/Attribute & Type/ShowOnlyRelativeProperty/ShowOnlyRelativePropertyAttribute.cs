using UnityEngine;

public class ShowOnlyRelativePropertyAttribute : PropertyAttribute {

    public string[] relativeProperties;

    public ShowOnlyRelativePropertyAttribute(string[] relativeProperties) {
        this.relativeProperties = relativeProperties;
    }
}