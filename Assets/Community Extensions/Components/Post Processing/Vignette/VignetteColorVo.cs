using UnityEngine;

namespace PostProcessing {
    [System.Serializable]
    public class VignetteColorVo : VignetteVo {
        [Space]
        [Range(0f,1f)]
        public float defaultIntensity = 0f;
        [Tooltip("Checks object by type")]
        [DropDownList("types", null, "InventoryManager", "GameController", new string[] { "None" }, true)]
        public int type;
        [DropDownList("items", "item", "InventoryManager", "GameController", new string[] { "None" })]
        public string name;
        public int priority = 0;
        public bool doLerp = true;
        [EnableCondition("doLerp", "true")]
        public float lerpSpeed = 0.1f;
    }
}