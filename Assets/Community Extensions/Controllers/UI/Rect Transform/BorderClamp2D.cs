using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderClamp2D : MonoBehaviour {

	public enum DetectOn { OnCollisionEnter, OnCollisionExit }

	public RectTransform rect;
	public Vector2 offset;
	public DetectOn detectOn;
	public bool skipFirstCall;

	[Space]
	public bool hasTargetX;
	[EnableCondition("hasTargetX", "true")]
	public float targetX;

	public bool hasTargetY;
	[EnableCondition("hasTargetY", "true")]
	public float targetY;

	protected void OnCollisionExit2D(Collision2D other) {
		if (detectOn == DetectOn.OnCollisionExit) {
			if (!skipFirstCall) {
				ClampRect();
			}
			skipFirstCall = false;
		}		
	}
	 
	protected void OnCollisionEnter2D(Collision2D other) {
		if (detectOn == DetectOn.OnCollisionEnter) {
			if (!skipFirstCall) {
				if (hasTargetX || hasTargetY) {
					Vector2 pos = rect.anchoredPosition;
					if (DoBorderValues(targetX, rect.anchoredPosition.x)) {
						pos.x = targetX;	
					}
					if (DoBorderValues(targetY, rect.anchoredPosition.y)) {
						pos.y = targetY;
					}
					rect.anchoredPosition = pos;
				} else {
					ClampRect();
				}
			}
			skipFirstCall = false;
		}
	}

	protected bool DoBorderValues(float targetValue, float source) {
		return (source <= targetValue) ? true : false;
	}

	protected void ClampRect() {
		rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + offset.x, rect.anchoredPosition.y + offset.y);
	}
}
