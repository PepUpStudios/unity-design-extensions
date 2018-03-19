using UnityEngine;
using UnityEngine.EventSystems;
using State.Event;

public class DetectInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public event EventDelegator OnPointerStart;
    public event EventDelegator OnPointerEnd;

    public void OnPointerEnter(PointerEventData eventData) {
        TriggerOnPointerStart();
    }

    public void OnPointerExit(PointerEventData eventData) {
        TriggerOnPointerEnd();
    }

    protected void TriggerOnPointerStart() {
        if(OnPointerStart != null) OnPointerStart();
    }

    protected void TriggerOnPointerEnd() {
        if(OnPointerEnd != null) OnPointerEnd();
    }
}