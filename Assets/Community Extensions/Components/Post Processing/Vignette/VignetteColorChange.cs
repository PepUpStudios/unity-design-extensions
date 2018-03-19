using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using State.Event;
using Inventory;

namespace PostProcessing {
    public class VignetteColorChange : EventManager {
        
        public VignetteColorManager manager;
        public VignettePulse pulse;
        public VignetteColorVo settings;

        public VignetteColorChange nextVignette { set; get; }

        protected VignetteColorChange previousVignette = null;
        
        public void DoColorChange(params object[] objs) {
            if (manager.vignette == null) return;
            try {
                ItemID item = (ItemID)objs[0];
                if (!IsValidateItem(item)) return;
            } catch {}
            if (pulse != null) pulse.DoStopPulse();
            ChangeColor();
        }

        public void DoDeactivate(params object[] objs) {
            if (manager.vignette == null) return;
            try {
                ItemID item = (ItemID)objs[0];
                if (!IsValidateItem(item)) return;
            } catch {}
            ChangeBackColor();
        }

        public void DoStartPulse(params float[] objs) {
            if (manager.currentVignette == this) {
                pulse.DoStartPulse();
            }
        }

        public void DoStopPulse(params float[] objs) {
            if (manager.currentVignette == this || previousVignette == null) {
                pulse.DoStopPulse();
            }
        }

        protected bool IsValidateItem(ItemID item) {
            int type = item.type + 1;
            string name = item.name;
            if ((settings.type != 0 && settings.type != type) ||
            (settings.name != "None" && settings.name != name)) {
                return false;
            }
            return true;
        }

        protected void ChangeColor() {
            VignetteColorChange currentVignette = manager.currentVignette;
            if (currentVignette == null || !(settings.priority < currentVignette.settings.priority)) {
                SetCurrentSettings();
                InitializeSettings(settings);
            } else {
                VignetteColorChange counterVignette = currentVignette;
                VignetteColorChange oneUpVignette = null;
                /*
                Need to loop through all the active vignette in order to find the appropriate priority
                position for the requested vignette.
                */
                while (counterVignette != null && counterVignette.settings.priority > settings.priority) {
                    oneUpVignette = counterVignette;
                    counterVignette = oneUpVignette.previousVignette;
                }
                if (counterVignette == null) {
                    oneUpVignette.previousVignette = this;
                    nextVignette = oneUpVignette;
                    previousVignette = null;
                } else {
                    nextVignette = counterVignette.nextVignette;
                    nextVignette.previousVignette = this;
                    counterVignette.nextVignette = this;
                    previousVignette = counterVignette;
                }
            }
        }

        protected void ChangeBackColor() {
            VignetteColorChange currentVignette = manager.currentVignette;
            if (currentVignette != null) {
                if (currentVignette == this) {
                    if (previousVignette != null) {
                        InitializeSettings(previousVignette.settings);
                    } else {
                        ResetSettings(settings);
                    }
                    ResetCurrentSettings();
                } else if (nextVignette != null) {
                    nextVignette.previousVignette = this.previousVignette;
                }
            }
        }

        protected void SetCurrentSettings() {
            VignetteColorChange currentVignette = manager.currentVignette;
            if (currentVignette != null) currentVignette.nextVignette = this;
            previousVignette = currentVignette;
            manager.currentVignette = this;
        }

        protected void ResetCurrentSettings() {
            VignetteColorChange currentVignette = manager.currentVignette;
            manager.currentVignette = previousVignette;
            previousVignette = null;
            nextVignette = null;
        }

        protected void InitializeSettings(VignetteColorVo obj) {
            Vignette vignette = manager.vignette;
            vignette.mode.value = obj.mode;
            vignette.center.value = obj.center;
            vignette.color.value = obj.color;
            vignette.smoothness.value = obj.smoothness;
            vignette.roundness.value = obj.roundness;
            vignette.rounded.value = obj.rounded;
            if (!obj.doLerp) vignette.intensity.value = obj.intensity;
            else manager.DoLerp();
        }

        protected void ResetSettings(VignetteColorVo obj) {
            Vignette vignette = manager.vignette;
            if (!obj.doLerp) vignette.intensity.value = obj.defaultIntensity;
            else manager.DoDefaultLerp();
        }
    }
}