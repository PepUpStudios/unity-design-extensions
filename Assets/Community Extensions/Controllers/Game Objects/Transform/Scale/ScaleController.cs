using UnityEngine;
using State.Event;

[RequireComponent(typeof(LerpScale))]
public class ScaleController : EventManager {

    public Transform transformObj;

	public Vector3[] scalePoints;

	public bool loopPoints = true;

	protected LerpScale lerp;

    protected int currentIndex;
    protected bool oneLoop;

	public event EventDelegator OnStart;
	public event EventDelegator OnEnd;

	void Start () {
		lerp = this.GetComponent<LerpScale>();
	}

	public override void OnEnable() {
		base.OnEnable();
        currentIndex = 0;
        oneLoop = false;
	}

	public virtual void DoFadeStart(params float[] args) {
		if(this.enabled) {
			TriggerOnStart();
            if (currentIndex == 0 && oneLoop) return;
			lerp.Begin(transformObj, scalePoints[currentIndex]);
            if (loopPoints) currentIndex = GetNextIndex(currentIndex, scalePoints.Length);
            oneLoop = true;
		}
	}

	public virtual void DoFadeEnd(params float[] args) {
		if(this.enabled) {			
			lerp.Begin(transformObj, scalePoints[currentIndex]);
			TriggerOnEnd();
            currentIndex = 0;
		}
	}

	public virtual void DoObjectStart(params object[] objs) {
		if(this.enabled) {
			TriggerOnStart();
            if (currentIndex == 0 && oneLoop) return;
			lerp.Begin(transformObj, scalePoints[currentIndex]);
            if (loopPoints) currentIndex = GetNextIndex(currentIndex, scalePoints.Length);
            oneLoop = true;
		}
	}

	public virtual void DoObjectEnd(params object[] objs) {
		if(this.enabled) {			
			lerp.Begin(transformObj, scalePoints[currentIndex]);
			TriggerOnEnd();
            currentIndex = 0;
		}
	}

    protected int GetNextIndex(int value, int len) {
        value++;
        return (value >= len ) ? 0 : value;
    }

	public override void OnDisable() {
		base.OnDisable();
		transformObj.localScale = scalePoints[currentIndex];
	}

	protected void TriggerOnStart() {
		if (OnStart != null) OnStart();
	}

	protected void TriggerOnEnd() {
		if (OnEnd != null) OnEnd();
	}
}