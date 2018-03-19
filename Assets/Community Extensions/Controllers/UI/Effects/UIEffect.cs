using State.Event;

namespace UnityEngine.UI.Extensions {
    public abstract class UIEffect : EventManager {

        protected MaskableGraphic mGraphic = null;

        public virtual void Start() {
            mGraphic = this.GetComponent<MaskableGraphic>();
        }

        public abstract void SetMaterial(params object[] objs);

        public void RemoveMaterial(params object[] objs) {
            if (mGraphic != null) {
                mGraphic.material = null;
            }
        }
    }
}