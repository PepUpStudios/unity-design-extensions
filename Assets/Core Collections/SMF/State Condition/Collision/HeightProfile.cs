using UnityEngine;

namespace State.Condition {
    [System.Serializable]
    public class HeightProfile {
        [Range(0, 20)]
        public int orderIndex;
        public eHeight height;
    }
}