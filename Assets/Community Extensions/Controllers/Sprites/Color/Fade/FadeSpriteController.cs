using UnityEngine;
using State.Event;

[RequireComponent(typeof(LerpSpriteFade))]
public class FadeSpriteController : EventManager {

    public SpriteRenderer sprite;

	public Color startColor;
	public Color endColor;

	protected LerpSpriteFade lerp;

	public event EventDelegator OnStart;
	public event EventDelegator OnEnd;

	public void Awake() {
		sprite.color = endColor;
	}

	void Start () {
		lerp = this.GetComponent<LerpSpriteFade>();
	}

	public virtual void DoFadeStart(params float[] args) {
		if(this.enabled) {
			TriggerOnStart();
			lerp.Begin(sprite, startColor);
		}
	}

	public virtual void DoFadeEnd(params float[] args) {
		if(this.enabled) {			
			lerp.Begin(sprite, endColor);
			TriggerOnEnd();
		}
	}

	public override void OnDisable() {
		base.OnDisable();
		sprite.color = endColor;
	}

	protected void TriggerOnStart() {
		if (OnStart != null) OnStart();
	}

	protected void TriggerOnEnd() {
		if (OnEnd != null) OnEnd();
	}
}