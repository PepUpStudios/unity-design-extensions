using UnityEngine;
using UnityEngine.UI;
using State.Event;

public class LerpUIColor : MonoBehaviour {

    protected Image source;
    protected float speed;
    protected Color targetValue;
    protected bool isDestroyed;

    public event EventDelegator OnEnd;

    public void Init(Image source, float speed) {
        this.source = source;
        this.speed = speed;
    }

    public void Init(Image source) {
        this.source = source;
    }

    public void LerpBegin(Color targetValue, float speed) {
        this.speed = speed;
        LerpBegin(targetValue);
    }

    public void LerpBegin(Color targetValue) {
        this.targetValue = targetValue;
        if (!isDestroyed) this.enabled = true;
    }

    public void OnDestroy() {
        isDestroyed = true;
    }

    public void Update() {
        this.source.color = Color.Lerp(this.source.color, targetValue, speed * Time.deltaTime);
        if (this.source.color == targetValue) {
            LerpEnd();
            TriggerOnEnd();
        }
    }

    public void LerpEnd() {
        this.enabled = false;
    }

    public void OnEnable() {
        isDestroyed = false;
    }

    public void OnDisable() {
        this.source.color = targetValue;
    }

    protected void TriggerOnEnd(params float[] args) {
        if (OnEnd != null) OnEnd(args);
    }
}