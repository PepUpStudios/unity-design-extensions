using UnityEngine;
using State.Event;
using UnityEngine.SceneManagement;

public abstract class UIEvent : EventManager {

    public virtual void Awake() {
        PlugEvents();
        SceneManager.sceneUnloaded += UnPlugEvents;
    }

    public override void OnEnable() {
    }

    public override void OnDisable() {
    }

    protected void UnPlugEvents(Scene scene) {
        UnPlugEvents();
        SceneManager.sceneUnloaded -= UnPlugEvents;
    }

    public void OnApplicationQuit() {
        UnPlugEvents();
    }
}