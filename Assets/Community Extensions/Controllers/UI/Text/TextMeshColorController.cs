using UnityEngine;
using TMPro;

public class TextMeshColorController : UIEvent {
    
    public float speed = 5f;
    public Color addColor = Color.white;
    public Color removeColor = Color.white;
    public bool overrideDefault = false;
    [EnableCondition("overrideDefault", "true")]
    public Color defaultColor = Color.white;

    protected TextMeshProUGUI textMesh = null;
    protected Color currTarget = Color.white;
    protected float steps = 0f;

    public void Start() {
        textMesh = this.GetComponent<TextMeshProUGUI>();
        if (!overrideDefault) defaultColor = textMesh.color;
        this.enabled = false;
    }

    public void DoAddColor(params object[] objs) {
        currTarget = addColor;
        steps = 0f;
        this.enabled = true;
    }

    public void DoRemoveColor(params object[] objs) {
        currTarget = removeColor;
        steps = 0f;
        this.enabled = true;
    }

    public void DoStopColor(params object[] objs) {
        currTarget = defaultColor;
        steps = 0f;
        this.enabled = true;
    }

    public void DoResetColor(params object[] objs) {
        textMesh.color = defaultColor;
        this.enabled = false;
    }

    public void Update() {
        steps += speed * Time.unscaledDeltaTime;
        textMesh.color = Color.Lerp(textMesh.color, currTarget, steps);
        if (steps > 1) {
            if (currTarget == defaultColor) { 
                this.enabled = false;
            } else {
                steps = 0;
                currTarget = defaultColor;
            }
        }
    }
}