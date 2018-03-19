using UnityEngine;
using State.Event;

[RequireComponent(typeof(LerpUIScale))]
public class ScaleUIController : EventManager {

	public RectTransform rect;

	public Vector3[] scalePoints;

	protected LerpUIScale lerp;

    protected int currentIndex;
    protected bool oneLoop;

	public event EventDelegator OnStart;
	public event EventDelegator OnEnd;

	void Start () {
		lerp = this.GetComponent<LerpUIScale>();
	}

	public override void OnEnable() {
		base.OnEnable();
        currentIndex = 0;
        oneLoop = false;
	}

	public virtual void DoScaleStart(params float[] args) {
		if(this.enabled) {
			TriggerOnStart();
            if (currentIndex == 0 && oneLoop) return;
			lerp.Begin(rect, scalePoints[currentIndex]);
            currentIndex = GetNextIndex(currentIndex, scalePoints.Length);
            oneLoop = true;
		}
	}

	public virtual void DoFirstScaleStart(params float[] args) {
		if(this.enabled) {
			TriggerOnStart();
			lerp.Begin(rect, scalePoints[0]);
		}
	}

	public virtual void DoScaleEnd(params float[] args) {
		if(this.enabled) {			
			lerp.Begin(rect, scalePoints[currentIndex]);
			TriggerOnEnd();
            currentIndex = 0;
			oneLoop = false;
		}
	}

	public virtual void DoLastScaleEnd(params float[] args) {
		if(this.enabled) {
			lerp.Begin(rect, scalePoints[scalePoints.Length - 1]);
			TriggerOnEnd();
		}
	}

	public virtual void DoForceScaleEnd(params float[] args) {
		if(this.enabled) {			
			lerp.enabled = false;
			TriggerOnEnd();
            currentIndex = 0;
			oneLoop = false;
		}
	}

    protected int GetNextIndex(int value, int len) {
        value++;
        return (value >= len ) ? 0 : value;
    }

	public override void OnDisable() {
		base.OnDisable();
		rect.localScale = scalePoints[currentIndex];
	}

	protected void TriggerOnStart() {
		if (OnStart != null) OnStart();
	}

	protected void TriggerOnEnd() {
		if (OnEnd != null) OnEnd();
	}
}
