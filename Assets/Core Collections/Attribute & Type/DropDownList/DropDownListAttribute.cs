using UnityEngine;
using System.Collections.Generic;

public class DropDownListAttribute : PropertyAttribute {

    public string[] gameObjectTag = null;
    public string[] componentName = null;
    public string[] propertyName = null;
    public string[] propertyRelativeName = null;
    public string[] defaultKeys = null;
    public bool indexValue = false;
    public string[] prefix = new string [] { "" };

    public DropDownListAttribute(string[] propertyName, string[] propertyRelativeName,
        string[] componentName, string[] gameObjectTag, string[] defaultKeys,
        bool getIndexValue, string[] prefix) {
        this.gameObjectTag = gameObjectTag;
        this.componentName = componentName;
        this.propertyName = propertyName;
        this.propertyRelativeName = propertyRelativeName;
        this.defaultKeys = defaultKeys;
        this.indexValue = getIndexValue;
        this.prefix = prefix;
    }

    public DropDownListAttribute(string propertyName, string propertyRelativeName = null,
        string componentName = "this", string gameObjectTag = "this", string[] defaultKeys = null,
        bool getIndexValue = false, string prefix = "") {
        this.gameObjectTag = new string[] { gameObjectTag };
        this.componentName = new string[] { componentName };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { propertyRelativeName };
        this.defaultKeys = defaultKeys;
        this.indexValue = getIndexValue;
        this.prefix = new string[] { prefix };
    }

    public DropDownListAttribute(string propertyName) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = null;
        this.indexValue = false;
    }

    public DropDownListAttribute(string prefix, string propertyName) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = null;
        this.indexValue = false;
        this.prefix = new string[] { prefix };
    }

    public DropDownListAttribute(string propertyName, bool getIndexValue = false) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = null;
        this.indexValue = getIndexValue;
    }

    public DropDownListAttribute(string prefix, string propertyName, bool getIndexValue = false) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = null;
        this.indexValue = getIndexValue;
        this.prefix = new string[] { prefix };
    }

    public DropDownListAttribute(string propertyName, string[] defaultKeys = null, bool getIndexValue = false) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = null;
        this.indexValue = getIndexValue;
    }

    public DropDownListAttribute(string prefix, string propertyName, string[] defaultKeys = null, bool getIndexValue = false) {
        this.gameObjectTag = new string[] { "this" };
        this.componentName = new string[] { "this" };
        this.propertyName = new string[] { propertyName };
        this.propertyRelativeName = new string[] { null };
        this.defaultKeys = defaultKeys;
        this.indexValue = getIndexValue;
        this.prefix = new string[] { prefix };
    }
}
