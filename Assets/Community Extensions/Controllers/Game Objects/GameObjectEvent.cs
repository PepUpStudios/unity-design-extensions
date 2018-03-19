using UnityEngine;
using State.Event;

public class GameObjectEvent : EventManager {

    public GameObject gameObj;

    public event EventDelegator OnActivate;
    public event EventDelegator OnDeactivate;

    public void DoActivate(params float[] args) {
        this.gameObj.SetActive(true);
    }

    public void DoDeactivate(params float[] args) {
        this.gameObj.SetActive(false);
    }

    public void DoObjectActivate(params object[] objs) {
        this.gameObj.SetActive(true);
    }

    public void DoObjectDeactivate(params object[] objs) {
        this.gameObj.SetActive(false);
    }

    public override void OnEnable() {
        base.OnEnable();
        TriggerOnActivate();
    }

    public override void OnDisable() {
        base.OnDisable();
        TriggerOnDeactivate();
    }

    protected void TriggerOnDeactivate(params float[] args) {
        if (OnDeactivate != null) OnDeactivate(args);
    }

    protected void TriggerOnActivate(params float[] args) {
        if (OnActivate != null) OnActivate(args);
    }
}