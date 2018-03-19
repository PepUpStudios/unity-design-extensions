using UnityEngine;
using State.Event;

[RequireComponent(typeof(LerpUIFade))]
public class FadeUIController : EventManager {

	public CanvasGroup canvasGroup;

	public float startAlpha;
	public float endAlpha;

	protected LerpUIFade lerp;
	protected float currAlpha;
	protected bool toggle;

	public event EventDelegator OnStart;
	public event EventDelegator OnEnd;

	public void Awake() {
		this.canvasGroup = this.GetComponent<CanvasGroup>();
		canvasGroup.alpha = endAlpha;
	}

	void Start () {
		lerp = this.GetComponent<LerpUIFade>();
		currAlpha = endAlpha;
		toggle = false;
	}

	public virtual void DoFadeStart(params float[] args) {
		if(this.enabled) {
			TriggerOnStart();
			lerp.Begin(canvasGroup, startAlpha);
		}
	}

	public virtual void DoFadeEnd(params float[] args) {
		if(this.enabled) {			
			lerp.Begin(canvasGroup, endAlpha);
			TriggerOnEnd();
		}
	}

	public virtual void DoFadeToggleStart(params float[] args) {
		if(this.enabled) {
			toggle = true;
			DoFadeToggle();
		}
	}

	public virtual void DoFadeToggle(params float[] args) {
		if(this.enabled && toggle) {
			currAlpha = GetAlpha(currAlpha);
			lerp.Begin(canvasGroup, currAlpha);
		}
	}

	public virtual void DoFadeToggleEnd(params float[] args) {
		if(this.enabled) {
			toggle = false;
		}
	}

	public override void OnDisable() {
		base.OnDisable();
		canvasGroup.alpha = endAlpha;
	}

	protected void TriggerOnStart() {
		if (OnStart != null) OnStart();
	}

	protected void TriggerOnEnd() {
		if (OnEnd != null) OnEnd();
	}

	protected float GetAlpha(float value) {
		return (value == startAlpha) ? endAlpha : startAlpha;
	}
}
