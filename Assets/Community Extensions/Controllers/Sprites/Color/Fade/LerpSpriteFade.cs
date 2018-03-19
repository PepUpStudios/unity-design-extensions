using UnityEngine;
using State.Event;

public class LerpSpriteFade : MonoBehaviour {

    public float speed;

	protected SpriteRenderer source;
	protected Color targetValue;

    public event EventDelegator OnLerpFinish;

	public void Begin(SpriteRenderer source, Color targetValue) {
		this.source = source;
		this.targetValue = targetValue;
		this.enabled = true;
	}

    protected void TriggerOnLerpFinish() {
		if (OnLerpFinish != null) OnLerpFinish();
	}
	
	protected void Update () {
		source.color = Color.Lerp(source.color, targetValue, Time.deltaTime * speed);
		if (source.color == targetValue) {
			this.enabled = false;
            TriggerOnLerpFinish();
		}
	}

	protected void OnDisable() {
		this.enabled = false;
	}
}