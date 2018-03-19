using UnityEngine;

public class FadeAlertUIController : FadeUIController {

    protected FindPlayerCaller obj;

    public void AddEvents(FindPlayerCaller obj) {
        this.obj = obj;
        obj.OnSight += DoFadeStart;
	}

    public void RemoveEvents() {
        obj.OnSight -= DoFadeStart;
	}

    public override void OnDisable() {
        base.OnDisable();
        RemoveEvents();
    }
}