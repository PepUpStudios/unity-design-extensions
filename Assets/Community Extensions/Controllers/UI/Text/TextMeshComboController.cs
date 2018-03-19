using UnityEngine;
using System.Collections.Generic;

public class TextMeshComboController : TextMeshController {    

    [Space]
    public ComboMap[] comboMaps;
    public Dictionary<float, ComboMap> comboMap;

    protected ComboCount comboCount;

    public void Awake() {
        comboMap = new Dictionary<float, ComboMap>();
        comboCount = GameManager.instance.GetComboCount();

        foreach(ComboMap combo in comboMaps) {
            comboMap.Add(combo.atCombo, combo);
        }
    }

    public override void OnEnable() {
        base.OnEnable();
        float score = comboCount.score;
        int scoreInt = (int)(score / 10) * 10;
        ComboMap combo = GetCombo(scoreInt);
        if (combo != null) this.textMesh.color = combo.color;
        this.DoFloatTextUpdate(score);
    }

    protected ComboMap GetCombo(float key) {
        return (comboMap.ContainsKey(key)) ? comboMap[key] : null;
    }
}

[System.Serializable]
public class ComboMap {
    public float atCombo;
    public Color color;
}