using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIChangeImageSprite : UIEvent {
    
    public Sprite targetSprite;
    public bool overrideDefault = false;
    [EnableCondition("overrideDefault", "true")]
    public Sprite defaultSprite;

    protected Image imageComponent = null;

    public void Start() {
        imageComponent = this.GetComponent<Image>();
        if (!overrideDefault) defaultSprite = imageComponent.sprite;
    }

    public void DoTargetSprite(params object[] objs) {
        imageComponent.sprite = targetSprite;
    }

    public void DoDefaultSprite(params object[] objs) {
        imageComponent.sprite = defaultSprite;
    }
}