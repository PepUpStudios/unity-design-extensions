using UnityEngine;
using UnityEngine.UI;
using State.Event;

[RequireComponent(typeof(LerpSlider))]
public class SliderUIController : EventManager {

	public Slider slider;

	protected LerpSlider lerp;

	public void Start() {
		lerp = this.GetComponent<LerpSlider>();
	}

	public void DoInit(params float[] args) {
		slider.maxValue = args[0];
		slider.value = slider.maxValue;
	}

	public void SetSliderValue(params float[] args) {
		float targetValue = args[0];
		lerp.Begin(slider, targetValue);
	}

	public override void OnDisable() {
		base.OnDisable();
		slider.value = slider.maxValue;
	}
}
