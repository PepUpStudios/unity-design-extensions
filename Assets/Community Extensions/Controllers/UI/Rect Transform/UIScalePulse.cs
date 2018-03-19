using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIScalePulse : UIEvent {
    
    public float speed = 5f;
    public Vector3 targetScale = Vector3.one;
    public bool overrideDefault = false;
    [EnableCondition("overrideDefault", "true")]
    public Vector3 defaultScale = Vector3.one;

    protected Vector3 currTargetScale;
    protected RectTransform rectComponent = null;
    protected bool isDefaultStop = false;

    public void Start() {
        rectComponent = this.GetComponent<RectTransform>();
        if (!overrideDefault) defaultScale = rectComponent.localScale;
        currTargetScale = targetScale;
        isDefaultStop = false;
        this.enabled = false;
    }

    public void DoStartPulse(params object[] objs) {
        isDefaultStop = false;
        this.enabled = true;
    }

    public void DoStopPulse(params object[] objs) {
        isDefaultStop = false;
        this.enabled = false;
    }

    public void DoDefaultStopPulse(params object[] objs) {
        isDefaultStop = true;
        currTargetScale = defaultScale;
        this.enabled = true;
    }

    public void DoResetPulse(params object[] objs) {
        this.enabled = false;
        rectComponent.localScale = defaultScale;
    }

    public void Update() {
        Pulse();
    }

    float currStep = 0;

    public void Pulse() {
        currStep = GetSteps();
        rectComponent.localScale = Vector3.LerpUnclamped(rectComponent.localScale, currTargetScale, currStep);
        if (Vector3.Distance(rectComponent.localScale, currTargetScale) <= 0.01f) {
            currTargetScale = GetTargetScale();
            if (isDefaultStop) this.enabled = false;
        }
    }

    protected float GetSteps() {
        return Mathf.PingPong(Time.unscaledDeltaTime * speed, 1);
    }

    protected Vector3 GetTargetScale() {
        return (Vector3.Distance(rectComponent.localScale, targetScale) <= 0.01f) ? defaultScale : targetScale;
    }
}