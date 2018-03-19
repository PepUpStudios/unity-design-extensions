using UnityEngine;
using UnityEngine.UI;
using State.Event;

public class LerpSlider : MonoBehaviour {

    public float speed;

    protected Slider slider;   
    protected float targetValue;

    public event EventDelegator OnStart;
	public event EventDelegator OnEnd;

    public void TriggerOnFadeStart() {
		if (OnStart != null) {
			OnStart();
		}
	}

	public void TriggerOnFadeEnd() {
		if (OnEnd != null) {
			OnEnd();
		}
	}

    public void Begin(Slider slider, float targetValue) {
        TriggerOnFadeStart();
        this.slider = slider;
        this.targetValue = targetValue;
        this.enabled = true;        
    }

    public void Update() {
        slider.value = Mathf.Lerp(slider.value, targetValue, Time.deltaTime * speed);
        if (Mathf.Abs(slider.value - targetValue) <= 0.1f) {
            TriggerOnFadeEnd();
            this.enabled = false;
        }
    }

    protected void OnDisable() {
		this.enabled = false;
	}
}