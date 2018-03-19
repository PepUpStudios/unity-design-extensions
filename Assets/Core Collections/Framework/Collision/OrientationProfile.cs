using UnityEngine;

namespace State.Condition {
    [System.Serializable]
    public class OrientationProfile {
        [Range(0, 20)]
        public int orderIndex;
        public eOrientation orientation;
        [Tooltip("If left '0' on hit collision is used instead of raycast")]
        [Range(0, 5)]
        public float rayLenght = 0;
    }
}