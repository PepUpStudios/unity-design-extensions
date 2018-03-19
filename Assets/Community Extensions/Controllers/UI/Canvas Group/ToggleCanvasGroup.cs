using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class ToggleCanvasGroup : MonoBehaviour {

    public float Alpha1;
    public float Alpha2;

    [Space]
    public bool interactable1;
    public bool interactable2;

    protected CanvasGroup canvasGroup;
    protected bool toggleAlpha = false;
    protected bool toggleInteractable = false;

    protected void Start() {
        canvasGroup = this.GetComponent<CanvasGroup>();

        toggleAlpha = (canvasGroup.alpha == Alpha1) ? true : false;
        toggleInteractable = (canvasGroup.interactable == interactable1) ? true : false;
    }

    public void ToggleAlpha() {
        canvasGroup.alpha = (toggleAlpha) ? Alpha2 : Alpha1;
        toggleAlpha = !toggleAlpha;
    }

    public void ToggleInteractable() {
        canvasGroup.interactable = (toggleInteractable) ? interactable2 : interactable1;
        toggleInteractable = !toggleInteractable;
    }
}