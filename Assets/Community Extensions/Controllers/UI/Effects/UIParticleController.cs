using UnityEngine;
using State.Event;

public class UIParticleController : EventManager {

    public bool overrideDefault = false;
    [EnableCondition("overrideDefault", "true")]
    public ParticleSystem particle;

    public void Start() {
        if (particle == null) particle = this.GetComponent<ParticleSystem>();
        particle.Stop();
    }

    public void OnPlay(params object[] objs) {
        particle.Play();
    }

    public void OnPlayFloat(params float[] objs) {
        particle.Play();
    }

    public void OnStop(params object[] objs) {
        particle.Stop();
    }

    public void OnStopFloat(params float[] objs) {
        particle.Stop();
    }

    public void Play() {
        particle.Stop();
        particle.Play();
    }

    public void Stop() {
        particle.Stop();
    }
}