using UnityEngine;
using State.Event;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AutoDestructParticle : EventManager {

    public bool OnlyDeactivate;

    public bool addToPool;
    [EnableCondition("addToPool", "true")]
    public string poolType;

    [Tooltip("Default to particle duration when set false")]
    public bool onTime;
    [EnableCondition("onTime", "true")]
    public float duration;

    protected ObjectPool pool;

    public event EventDelegator OnDeactive;

    protected float counter;

    private void Start() {
        pool = ObjectPool.instance;
    }

    public override void OnEnable() {
        base.OnEnable();
        StartCoroutine("CheckIfAlive");
    }

    public override void OnDisable() {
        base.OnDisable();
        if (this.gameObject.activeInHierarchy) {
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator CheckIfAlive() {
        ParticleSystem ps = this.GetComponent<ParticleSystem>();

        while (true && ps != null) {
            yield return new WaitForSeconds(0.5f);
            counter += 0.5f;
            if (!ps.IsAlive(true) || (onTime && counter >= duration)) {
                if (OnlyDeactivate) {
                    this.gameObject.SetActive(false);
                    if (OnDeactive != null) {
                        OnDeactive();
                    }
                } else if (addToPool) {
                    this.gameObject.transform.parent = null;
                    pool.PutObject(poolType, this.gameObject.name, ps);
                    ps.gameObject.SetActive(false);
                } else {
                    GameObject.Destroy(this.gameObject);
                }
                break;
            }
        }
    }
}
