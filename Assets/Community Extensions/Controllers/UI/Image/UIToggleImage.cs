using UnityEngine.UI;

public class UIToggleImage : UIEvent {

    protected Image imageComponent = null;

    public void Start() {
        imageComponent = this.GetComponent<Image>();
    }
    
    public void DoActivate(params object[] objs) {
        imageComponent.enabled = true;
    }

    public void DoDeactivate(params object[] objs) {
        imageComponent.enabled = false;
    }
}