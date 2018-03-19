using UnityEngine;
using UnityEngine.UI;
using State.Event;

public class UpdateText : EventManager {

    protected Text text;

    public void Awake() {
        text = this.GetComponent<Text>();
    }

    public void UpdateTextBySlider(params float[] args) {
        this.text.text = args[0].ToString("F2");
    }
}