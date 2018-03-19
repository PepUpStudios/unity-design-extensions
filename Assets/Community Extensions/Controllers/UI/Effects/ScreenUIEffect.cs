
namespace UnityEngine.UI.Extensions {
    [AddComponentMenu("UI/Effects/Extensions/UIScreenEffect")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class ScreenUIEffect : UIEffect {

        public Shader shader;

        public override void SetMaterial(params object[] objs) {
            if (mGraphic != null) {
                if (mGraphic.material == null || mGraphic.material.name == "Default UI Material") {
                    //Applying default material with UI Image Crop shader
                    mGraphic.material = new Material(shader);
                }
            } else {
                Debug.LogError("Please attach component to a Graphical UI component");
            }
        }
    }
}