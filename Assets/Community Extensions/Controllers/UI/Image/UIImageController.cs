using UnityEngine;
using UnityEngine.UI;
using State.Event;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(LerpUIColor))]
public class UIImageController : EventManager {

    public Color targetColor = Color.white;
    public float lerpSpeed;

    protected Image imageComponent;
    protected Color defaultColor = Color.white;
    protected LerpUIColor lerp;

    public void Start() {
        imageComponent = this.GetComponent<Image>();
        defaultColor = imageComponent.color;
        lerp = this.GetComponent<LerpUIColor>();
        lerp.Init(imageComponent);
    }

    public void OnColorChange(params object[] objs) {
        Color newColor = (Color)objs[0];
        SetImageColor(newColor);
    }

    public void OnTargetColorChange(params object[] objs) {
        SetImageColor(targetColor);
    }

    public void OnRGBColorChange(params object[] objs) {
        Color newColor = (Color)objs[0];
        newColor.a = imageComponent.color.a;
        SetImageColor(newColor);
    }

    public void OnDefaultColorChange(params object[] objs) {
        SetImageColor(defaultColor);
    }

    public void OnDefaultRGBColorChange(params object[] objs) {
        Color newColor = defaultColor;
        newColor.a = imageComponent.color.a;
        SetImageColor(newColor);
    }

    public void OnColorChangeLerp(params object[] objs) {
        Color newColor = (Color)objs[0];
        float speed = GetSpeed(1, objs);
        lerp.LerpBegin(newColor, speed);
    }

    public void OnTargetColorChangeLerp(params object[] objs) {
        float speed = GetSpeed(0, objs);
        lerp.LerpBegin(targetColor, speed);
    }

    public void OnTargetRGBColorChangeLerp(params object[] objs) {
        Color newColor = targetColor;
        float speed = GetSpeed(0, objs);
        newColor.a = imageComponent.color.a;
        lerp.LerpBegin(newColor, speed);
    }

    public void OnRGBColorChangeLerp(params object[] objs) {
        Color newColor = (Color)objs[0];
        float speed = GetSpeed(1, objs);
        newColor.a = imageComponent.color.a;
        lerp.LerpBegin(newColor, speed);
    }

    public void OnDefaultColorChangeLerp(params object[] objs) {
        Color newColor = defaultColor;
        float speed = GetSpeed(0, objs);
        lerp.LerpBegin(newColor, speed);
    }

    public void OnDefaultRGBColorChangeLerp(params object[] objs) {
        Color newColor = defaultColor;
        float speed = GetSpeed(0, objs);
        newColor.a = imageComponent.color.a;
        lerp.LerpBegin(newColor, speed);
    }

    public void OnDefaultTargetRGBColorChangeLerp(params object[] objs) {
        Color newColor = defaultColor;
        float speed = GetSpeed(0, objs);
        newColor.a = imageComponent.color.a;
        lerp.LerpBegin(newColor, speed);
    }

    protected void SetImageColor(Color newColor) {
        imageComponent.color = newColor;
    }

    protected float GetSpeed(int index, params object[] objs) {
        float speed = lerpSpeed;
        if (objs.Length > 0) {
            try {
                speed = (float)objs[index];
            } catch {}
        }
        return speed;
    }
}