using UnityEngine;
using State.Event;

public class HealthManager : EventManager {

    [SerializeField]
    protected float health;
    [SerializeField]
    protected float min = 0;
    [SerializeField]
    protected float max = 100;

    protected int index = 0;
    
    public event EventDelegator OnStart;
    public event EventDelegator OnUpdate;
    public event EventDelegator OnEnd;
    public event EventDelegator OnReset;

    public void Start() {        
        if (OnStart != null) {
            OnStart(health);
        }
    }

    public void DoReduce(params float[] args) {
        float amount = args[0];
        ReduceBy(amount);
    }

    protected void ReduceBy(float amount) {
        health = (health - amount) <= min ? min : (health - amount);
        DoDelegate();
    }

    public void DoAdd(params float[] args) {
        float amount = args[0];
        AddBy(amount);
    }

    public void DoReset(params float[] args) {
        AddBy(max);
    }

    protected void AddBy(float amount) {
        health = (health + amount) >= max ? max : (health + amount);
        DoDelegate();
    }

    protected void DoDelegate () {
        if (OnUpdate != null) {
            OnUpdate(health);
        }
        if (health == min && OnEnd != null) {
            OnEnd(health);
        }
        if (OnReset != null && health == max) {
            OnReset(health);
        }
    }

}
