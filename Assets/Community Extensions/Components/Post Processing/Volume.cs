using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace  PostProcessing {
    [RequireComponent(typeof(PostProcessLayer))]
    public class Volume : MonoBehaviour {
        public PostProcessProfile profile;
        public bool isGlobal = true;
        public float priority;
        [Range(0,1)]
        public float weight;
        [SerializeField, Layer]
        public int layer;

        public PostProcessVolume postVolume { get { return m_volume; } }
        public PostProcessProfile internalProfile { 
            get { 
                if (m_InternalProfile == null) m_InternalProfile = GetProfile();
                return m_InternalProfile; 
            } 
        }

        private PostProcessVolume m_volume = null;
        private PostProcessProfile m_InternalProfile = null;

        public void Start() {
            CreateVolume(internalProfile.settings.ToArray());
        }

        public void CreateVolume(params PostProcessEffectSettings[] settings) {
            m_volume = PostProcessManager.instance.QuickVolume(layer, priority, settings);
            m_volume.isGlobal = isGlobal;
            m_volume.weight = weight;
            m_volume.priority = priority;
        }

        private PostProcessProfile GetProfile() {
            PostProcessProfile internalProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
            if (profile != null) {
                foreach (var item in profile.settings) {
                    var itemCopy = Instantiate(item);
                    internalProfile.settings.Add(itemCopy);
                }
            }
            return internalProfile;
        }

        public void OnApplicationQuit() {
            RuntimeUtilities.DestroyVolume(postVolume, false);
        }
    }
}