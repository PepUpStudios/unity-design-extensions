using UnityEngine;
using State.Event;

public class LerpScale : MonoBehaviour {

    public float speed;

	protected Vector3 targetValue;
    protected Transform source;

	public event EventDelegator OnLerpFinish;

	public void Begin(Transform source, Vector3 targetValue) {
        this.source = source;
		this.targetValue = targetValue;
		this.enabled = true;
	}

	protected void TriggerOnLerpFinish() {
		if (OnLerpFinish != null) OnLerpFinish();
	}
	
	protected void Update () {
		this.source.localScale = Vector3.Lerp(this.source.localScale, targetValue, Time.deltaTime * speed);
		if (this.source.localScale == targetValue) {
			this.enabled = false;
			TriggerOnLerpFinish();
		}
	}

	protected void OnDisable() {
		this.enabled = false;
	}
}