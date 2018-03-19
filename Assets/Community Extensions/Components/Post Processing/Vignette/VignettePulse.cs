using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using State.Event;

namespace PostProcessing {
    public class VignettePulse : EventManager {
        public VignetteColorManager manager;
        public float speed = 5f;
        [Range(0f,1f)]
        public float defaultIntensity;

        protected float targetintensity;
        protected float m_DefaultIntensity;

        public void Awake() {
            PlugEvents();
            SceneManager.sceneUnloaded += UnPlugEvents;
            this.enabled = false;
        }

        public override void OnEnable() {
        }

        public override void OnDisable() {
        }

        public void DoStartPulse(params float[] args) {
            targetintensity = manager.currentVignette.settings.intensity;
            m_DefaultIntensity = defaultIntensity;
            if (m_DefaultIntensity > targetintensity) {
                m_DefaultIntensity = targetintensity;
                targetintensity = defaultIntensity;
            }
            targetintensity = targetintensity - m_DefaultIntensity;
            this.enabled = true;
        }

        public void DoStopPulse(params float[] args) {
            this.enabled = false;
        }

        void Update() {
            manager.vignette.intensity.value = Mathf.PingPong(speed * Time.time, targetintensity) + m_DefaultIntensity;
        }

        public void OnApplicationQuit() {
            UnPlugEvents();
            SceneManager.sceneUnloaded -= UnPlugEvents;
        }

        protected void UnPlugEvents(Scene scene) {
            UnPlugEvents();
            SceneManager.sceneUnloaded -= UnPlugEvents;
        }
    }
}