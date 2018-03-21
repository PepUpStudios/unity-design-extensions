using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace State.Event {
    public delegate void ObjectEventDelegator(params object[] args);
    public delegate void EventDelegator(params float[] args);

    public class EventManager : MonoBehaviour {//Heavily depended on editor.

        [HideInInspector]
        [SerializeField]
        public List<EventVo> eventObjs;

        public virtual void OnEnable() {
            PlugEvents();
        }

        public virtual void OnDisable() {
            UnPlugEvents();
        }

        public void PlugEvents() {
            for (int i = 0; i < eventObjs.Count; i++) {
                string targetMethodName = eventObjs[i].targetMethod;
                EventInfo sourceType = eventObjs[i].sourceComponent.GetType().GetEvent(eventObjs[i].sourceEvent);
                Delegate delegateRef = Delegate.CreateDelegate(sourceType.EventHandlerType, this, targetMethodName, true, true);
                MethodInfo eventMethod = sourceType.GetAddMethod();
                eventMethod.Invoke(eventObjs[i].sourceComponent, new[] {delegateRef});
                //sourceType.AddEventHandler(eventObjs[i].sourceComponent, delegateRef); //Old Code does not work with AOT compiler
            }
        }

        public void UnPlugEvents() {
            for (int i = 0; i < eventObjs.Count; i++) {
                string targetMethodName = eventObjs[i].targetMethod;
                EventInfo sourceType = eventObjs[i].sourceComponent.GetType().GetEvent(eventObjs[i].sourceEvent);
                Delegate delegateRef = Delegate.CreateDelegate(sourceType.EventHandlerType, this, targetMethodName, true, true);
                MethodInfo eventMethod = sourceType.GetRemoveMethod();
                eventMethod.Invoke(eventObjs[i].sourceComponent, new[] {delegateRef});
                //sourceType.RemoveEventHandler(eventObjs[i].sourceComponent, delegateRef); //Old Code does not work with AOT compiler
            }
        }
    }

    [Serializable]
    public class EventVo {
        public GameObject sourceObj;
        public UnityEngine.Object sourceComponent;
        public string sourceEvent;
        public string targetMethod;
        
        #if UNITY_EDITOR
        public bool isFold;
        #endif
    }
}