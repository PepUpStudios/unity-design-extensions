using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImagePulseAlpha : UIEvent {

    public float speed = 0.4f;
    [Range(0f, 1f)]
    public float targetAlpha = 0f;
    public bool overrideDefault = false;
    [Range(0f, 1f)]
    [EnableCondition("overrideDefault", "true")]
    public float defaultAlpha = 0f;

    protected Image imageComponent = null;
    protected Color currTargetColor;
    protected float currStep = 0f;

    public void Start() {
        imageComponent = this.GetComponent<Image>();
        Color color = imageComponent.color;
        if (!overrideDefault) defaultAlpha = color.a;
        currTargetColor = new Color(color.r, color.g, color.b, targetAlpha);
        this.enabled = false;
    }

    public void DoStartPulse(params object[] objs) {
        Color color = currTargetColor;
        color.r = imageComponent.color.r;
        color.g = imageComponent.color.g;
        color.b = imageComponent.color.b;
        color.a = targetAlpha;
        currTargetColor = color;
        this.enabled = true;
    }

    public void DoStopPulse(params object[] objs) {
        this.enabled = false;
    }

    public void DoResetPulse(params object[] objs) {
        this.enabled = false;
        Color color = currTargetColor;
        color.r = imageComponent.color.r;
        color.g = imageComponent.color.g;
        color.b = imageComponent.color.b;
        color.a = defaultAlpha;
        imageComponent.color = color;
    }

    public void Update() {
        Pulse();
    }

    public void Pulse() {
        currStep = GetSteps();
        imageComponent.color = Color.LerpUnclamped(imageComponent.color, currTargetColor, currStep);
        if (imageComponent.color == currTargetColor) {
            currTargetColor = GetTargetColor();
        }
    }

    protected float GetSteps() {
        return Mathf.PingPong(1, speed);
    }

    protected Color GetTargetColor() {
        Color color = currTargetColor;
        color.r = imageComponent.color.r;
        color.g = imageComponent.color.g;
        color.b = imageComponent.color.b;
        color.a = (Mathf.Abs(imageComponent.color.a - targetAlpha) < 0.1f)  ? defaultAlpha : targetAlpha;
        return color;
    }
}