using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImagePulseColor : UIEvent {
    
    public float speed = 0.4f;
    public Color targetColor = Color.white;
    public bool overrideDefault = false;
    [EnableCondition("overrideDefault", "true")]
    public Color defaultColor = Color.white;

    protected Image imageComponent = null;
    protected Color currTargetColor;
    protected float currStep = 0f;

    public void Start() {
        imageComponent = this.GetComponent<Image>();
        if (!overrideDefault) defaultColor = imageComponent.color;
        currTargetColor = targetColor;
        this.enabled = false;
    }

    public void DoStartPulse(params object[] objs) {
        this.enabled = true;
    }

    public void DoStopPulse(params object[] objs) {
        this.enabled = false;
    }

    public void DoResetPulse(params object[] objs) {
        this.enabled = false;
        imageComponent.color = defaultColor;
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
        return (imageComponent.color == targetColor) ? defaultColor : targetColor;
    }
}