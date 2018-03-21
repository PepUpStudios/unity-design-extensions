using UnityEngine;

namespace State.Condition {
    [System.Serializable]
    public class CollisionProfile {
        [Tooltip("Name the role of the state, not mandatory")]
        public string name;
        [Tooltip("Transition name of the state")]
        public string transitionName;
        [Tooltip("Priority of the state, must be unique")]
        [Range(0, 20)]
        public int orderID;
        public string onLandState;
    }
}