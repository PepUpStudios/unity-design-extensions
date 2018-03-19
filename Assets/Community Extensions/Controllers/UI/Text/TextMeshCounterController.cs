using UnityEngine;
using State.Event;
using TMPro;

public class TextMeshCounterController : EventManager {
    
    public TextMeshCounter counterTemplate;
    public string poolTag = "";

    public void DoAddCounter(params object[] objs) {
        string amount = (uint)objs[2] + "";
        TextMeshCounter counter = SpawnCounter(counterTemplate, transform.position, transform.rotation, transform);
        counter.Activate(amount);
    }

    protected TextMeshCounter SpawnCounter(TextMeshCounter template, Vector3 pos, Quaternion rot, Transform parent) {
        TextMeshCounter counter = ObjectPool.instance.GetObjectByPop<TextMeshCounter>(poolTag, template.name);
        if (counter == null) {
            counter = Instantiate(template, pos, rot, parent) as TextMeshCounter;
            //setting up attrs.
            counter.gameObject.name = template.gameObject.name;
        }
        counter.gameObject.SetActive(true);
        return counter;
    }
}