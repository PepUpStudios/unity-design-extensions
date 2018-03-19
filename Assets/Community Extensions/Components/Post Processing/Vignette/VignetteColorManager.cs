using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace PostProcessing {
    public class VignetteColorManager : MonoBehaviour {

        public Volume volume;
        
        public Vignette vignette { get { return m_vignette; } }
        public VignetteColorChange currentVignette { set; get; }

        protected PostProcessProfile profile = null;
        protected float targetIntensity;
        protected float speed;

        private Vignette m_vignette = null;

        public void Start() {
            profile = volume.internalProfile;
            m_vignette = GetVignette(profile);
            this.enabled = false;
        }
        
        protected Vignette GetVignette(PostProcessProfile profile) {
            Vignette vignette;
            profile.TryGetSettings(out vignette);
            return vignette;
        }

        public void DoLerp() {
            targetIntensity = currentVignette.settings.intensity;
            speed = currentVignette.settings.lerpSpeed;
            this.enabled = true;
        }

        public void DoDefaultLerp() {
            targetIntensity = currentVignette.settings.defaultIntensity;
            speed = currentVignette.settings.lerpSpeed;
            this.enabled = true;
        }

        public void Update() {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, targetIntensity, speed * Time.unscaledTime);
            if (Mathf.Abs(targetIntensity - vignette.intensity.value) < 0.01f) {
                this.enabled = false;
            }
        }
    }
}