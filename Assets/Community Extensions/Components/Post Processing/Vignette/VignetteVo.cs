namespace PostProcessing {
    [System.Serializable]
    public class VignetteVo {
        public UnityEngine.Rendering.PostProcessing.VignetteMode mode = UnityEngine.Rendering.PostProcessing.VignetteMode.Classic;
        public UnityEngine.Color color = UnityEngine.Color.white;
        public UnityEngine.Vector2 center = new UnityEngine.Vector2(0.5f,0.5f);
        [UnityEngine.Range(0f,1f)]
        public float intensity = 0f;
        [UnityEngine.Range(0f,1f)]
        public float smoothness = 0.2f;
        [UnityEngine.Range(0f,1f)]
        public float roundness = 0f;
        public bool rounded = false;
    }
}