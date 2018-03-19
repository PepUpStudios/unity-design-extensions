using UnityEngine;
using State.Event;

public class LerpUIFade : MonoBehaviour {

	public float speed;

	protected CanvasGroup canvasGroup;
	protected float targetValue;

	public event EventDelegator OnEnd;

	public void Begin(CanvasGroup canvasGroup, float targetValue) {
		this.canvasGroup = canvasGroup;
		this.targetValue = targetValue;
		this.enabled = true;
	}
	
	protected void Update () {
		canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetValue, Time.deltaTime * speed);
		if (Mathf.Abs(canvasGroup.alpha - targetValue) <= 0.01f) {
			this.enabled = false;
			TriggerOnEnd();
		}
	}

	protected void OnDisable() {
		this.enabled = false;
	}

	protected void TriggerOnEnd() {
		if (OnEnd != null) OnEnd();
	}
}
