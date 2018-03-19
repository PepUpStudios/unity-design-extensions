using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColorBySlider : MonoBehaviour {

    public Slider slider;
    public float targetValue;
    public Color color1;
    public Color color2;

    protected Image image;

    public void Start() {
        image = this.GetComponent<Image>();
        CheckColorChange();
    }

    public void CheckColorChange() {
        if (slider.value >= targetValue) {
            image .color = color1;
        } else {
            image .color = color2;
        }
    }
}