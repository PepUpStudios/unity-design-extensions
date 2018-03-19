using UnityEngine;
using State.Event;
using TMPro;

public class TextMeshController : EventManager {

    public TextMeshProUGUI textMesh;

    [Space]
    public string prefix = "";
    public string suffix = "";

    protected string currText = "";

    public void DoObjectTextUpdate(params object[] objs) {
        string text = objs[0] as string;
        textMesh.SetText(prefix + text + suffix);
    }

    public void DoFloatTextUpdate(params float[] args) {
        string text = args[0].ToString();
        textMesh.SetText(prefix + text + suffix);
    }
}