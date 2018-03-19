using State.Event;
using UnityEngine;

public class DetectObject : MonoBehaviour {

	public string targetTag;
	public bool disableOnStart;

	public event ObjectEventDelegator OnTriggerEnter;
	public event ObjectEventDelegator OnTriggerExit;
	public event ObjectEventDelegator OnCollisionEnter;
	public event ObjectEventDelegator OnCollisionExit;


	public void Start() {
		if (disableOnStart) enabled = false;
	}

	public void EnableTrigger(params float[] args) {
		this.enabled = true;
	}

	public void DisableTrigger(params float[] args) {
		this.enabled = false;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (!enabled && other.gameObject.tag != targetTag) return;
		if (OnTriggerEnter != null) OnTriggerEnter(other);
	}

	public void OnTriggerExit2D(Collider2D other) {
		if (!enabled && other.gameObject.tag != targetTag) return;
		if (OnTriggerExit != null) OnTriggerExit(other);
	}

	public void OnCollisionEnter2D(Collision2D other) {
		if (!enabled && other.gameObject.tag != targetTag) return;
		if (OnCollisionEnter != null) OnCollisionEnter(other);
	}

	public void OnCollisionExit2D(Collision2D other) {
		if (!enabled && other.gameObject.tag != targetTag) return;
		if (OnCollisionExit != null) OnCollisionExit(other);
	}
}
