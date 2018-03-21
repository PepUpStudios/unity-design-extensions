using System.Collections.Generic;
using UnityEngine;

namespace State.Condition {
    public class CollisionStateCondition : MonoBehaviour {

        public List<CollisionProfile> stateProfiles;
        public Dictionary<int, CollisionProfile> stateProfileMap;

        public List<HeightProfile> heights;
        public Dictionary<int, HeightProfile> stateHeightMap;

        public List<OrientationProfile> orientations;
        public Dictionary<int, OrientationProfile> stateOrientationMap;

        public List<CollisionOrderIndex> overrideStateProfile;
        public Dictionary<string, Dictionary<int, CollisionProfile>> overrideStateProfileMap;

        protected Dictionary<int, CollisionProfile> overridedStates = null;
        protected Vector3 zeroVector3 = Vector3.zero;

        public void Start() {
            this.stateProfileMap = new Dictionary<int, CollisionProfile>();
            foreach (CollisionProfile stateProfiles in this.stateProfiles) {
                this.stateProfileMap.Add(stateProfiles.orderID, stateProfiles);
            }
            this.stateHeightMap = new Dictionary<int, HeightProfile>();
            foreach (HeightProfile stateHeight in this.heights) {
                this.stateHeightMap.Add(stateHeight.height.GetHashCode(), stateHeight);
            }
            this.stateOrientationMap = new Dictionary<int, OrientationProfile>();
            foreach (OrientationProfile stateOrientation in this.orientations) {
                int orientation = stateOrientation.orientation.GetHashCode();
                this.stateOrientationMap.Add(orientation, stateOrientation);
            }
            this.overrideStateProfileMap = new Dictionary<string, Dictionary<int, CollisionProfile>>();
            foreach (CollisionOrderIndex orderIndex in this.overrideStateProfile) {
                this.overrideStateProfileMap.Add(orderIndex.name, new Dictionary<int, CollisionProfile>());
                List<CollisionProfile> stateProfileList = orderIndex.statesAllowed;
                foreach (CollisionProfile stateProfile in stateProfileList) {
                    this.overrideStateProfileMap[orderIndex.name].Add(stateProfile.orderID, stateProfile);
                }
            }
            if (orientations.Count > 0) {
                this.orientations.Sort(delegate (OrientationProfile p1, OrientationProfile p2) {
                        return p1.orderIndex.CompareTo(p2.orderIndex);
                    });
            }
        }

        public string GetStateByCollisionEnter(ObstacleProperty obstacle, string objName = null) {
            if (obstacle == null) {
                return null;
            }

            int height = obstacle.height.GetHashCode();                
            int orientation = Mathf.RoundToInt(Vector2.Angle(gameObject.transform.up, -obstacle.contactPoint.normal.normalized) / 45f);

            int heightOrderIndex = GetHeightIndex(height);
            int orientationOrderIndex = GetOrientationIndex(orientation);

            int finalStateIndex = this.EvaluateByOrder(this.stateProfileMap, heightOrderIndex, orientationOrderIndex);

            string finalState = (finalStateIndex != -1) ? this.stateProfileMap[finalStateIndex].transitionName : null;

            if (this.overrideStateProfileMap.TryGetValue(obstacle.type, out overridedStates)) {
                finalState = GetOverrideState(finalStateIndex, heightOrderIndex, orientationOrderIndex);
            }
            if (objName != null && this.overrideStateProfileMap.TryGetValue(objName, out overridedStates)) {
                finalState = GetOverrideState(finalStateIndex, heightOrderIndex, orientationOrderIndex);
            }
            return finalState;
        }

        public string GetLandStateByCollisionStay(ObstacleProperty obstacle, string objName = null) {
            int height = obstacle.height.GetHashCode();

            int heightOrderIndex = GetHeightIndex(height);
            string landState = (heightOrderIndex != -1) ? this.stateProfileMap[heightOrderIndex].onLandState : null;

            if (this.overrideStateProfileMap.TryGetValue(obstacle.type, out overridedStates)) {
                landState = this.GetOverrideState(heightOrderIndex);
            }
            if (objName != null && this.overrideStateProfileMap.TryGetValue(objName, out overridedStates)) {
                landState = this.GetOverrideState(heightOrderIndex);
            }
            return landState;
        }

        public string GetStateByRaycast(LayerMask layerMask) {
            int orientationOrderIndex = -1;
            foreach (OrientationProfile orientationProfile in this.orientations) {
                float lenght = orientationProfile.rayLenght;
                Vector3 orientationDir = GetRayAngle(orientationProfile.orientation);
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, orientationDir, lenght, layerMask.value);
                if (hit) {
#if UNITY_EDITOR
                    Debug.DrawRay(this.transform.position, orientationDir * lenght, Color.red);
#endif
                    orientationOrderIndex = orientationProfile.orderIndex;
                    break;
                }
#if UNITY_EDITOR
                Debug.DrawRay(this.transform.position, orientationDir * lenght, Color.yellow);
#endif
            }
            return (orientationOrderIndex != -1) ? this.stateProfileMap[orientationOrderIndex].transitionName : null;
        }

        protected int GetHeightIndex(int height) {
            HeightProfile heightProfile = null;
            int heightOrderIndex = -1; //cannot set to null so the value is -1
            if (this.stateHeightMap.TryGetValue(height, out heightProfile)) {
                heightOrderIndex = heightProfile.orderIndex;
            }
            return heightOrderIndex;
        }

        protected int GetOrientationIndex(int orientation) {
            OrientationProfile orientationProfile = null;
            int orientationOrderIndex = -1;
            if (this.stateOrientationMap.TryGetValue(orientation, out orientationProfile)) {
                orientationOrderIndex = orientationProfile.orderIndex;
            }
            return orientationOrderIndex;
        }

        protected string GetOverrideState(int index, int heightOrderIndex, int orientationOrderIndex) {
            if (!this.overridedStates.ContainsKey(index)) {
                if (this.overridedStates.ContainsKey(heightOrderIndex)) {
                    return overridedStates[heightOrderIndex].transitionName;
                } else if (this.overridedStates.ContainsKey(orientationOrderIndex)) {
                    return overridedStates[orientationOrderIndex].transitionName;
                } else {
                    return null;
                }
            }
            return overridedStates[index].transitionName;
        }

        protected string GetOverrideState(int index) {
            if (!this.overridedStates.ContainsKey(index)) {
                return null;
            }
            return overridedStates[index].onLandState;
        }

        protected int EvaluateByOrder(Dictionary<int, CollisionProfile> drawOrder, int valueA, int valueB, bool ascending = true) {
            if (valueA < 0 && valueB < 0) {
                return -1;
            } else if (valueB < 0) {
                return valueA;
            } else if (valueA < 0) {
                return valueB;
            } else if (valueA == valueB) {
                return valueA;
            } else if ((ascending && valueA < valueB) || (!ascending && valueA > valueB)) {
                return valueA;
            } else if ((ascending && valueB < valueA) || (!ascending && valueB > valueA)) {
                return valueB;
            }
            return -1;
        }

        protected Vector3 GetRayAngle(eOrientation orientation) {
            if (orientation == eOrientation.up) {
                return (transform.up.normalized);
            } else if (orientation == eOrientation.down) {
                return (-transform.up.normalized);
            } else if (orientation == eOrientation.right) {
                return (transform.right.normalized);
            } else if (orientation == eOrientation.left) {
                return (-transform.right.normalized);
            } else if (orientation == eOrientation.downRight) {
                return (-transform.up + transform.right).normalized;
            } else if (orientation == eOrientation.left) {
                return (-transform.up - transform.right).normalized;
            } else if (orientation == eOrientation.upRight) {
                return (transform.up + transform.right).normalized;
            } else if (orientation == eOrientation.upLeft) {
                return (transform.up - transform.right).normalized;
            }
            return this.zeroVector3;
        }
    }
}