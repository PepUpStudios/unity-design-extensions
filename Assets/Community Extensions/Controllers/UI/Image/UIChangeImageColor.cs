using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIChangeImageColor : UIEvent {

	public float speed = 0.4f;
	public Color targetColor = Color.white;
	public bool overrideDefault = false;
	[EnableCondition("overrideDefault", "true")]
	public Color defaultColor = Color.white;

	protected Color currTargetColor;
	protected Image imageComponent;

	public void Start() {
		imageComponent = this.GetComponent<Image>();
		if (!overrideDefault) imageComponent.color = defaultColor;
		currTargetColor = targetColor;
		this.enabled = false;
	}

	public void DoRGBColorChange(params float[] args) {
		imageComponent.color = targetColor;
	}

	public void DoColorReset(params float[] args) {
		imageComponent.color = defaultColor;
	}
}
