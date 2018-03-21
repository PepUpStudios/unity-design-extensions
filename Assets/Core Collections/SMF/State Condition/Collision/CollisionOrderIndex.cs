using UnityEngine;
using System.Collections.Generic;

namespace State.Condition {
    [System.Serializable]
    public class CollisionOrderIndex {

        [Tooltip("Toggle between state or obstacle name: true equals state Name and false equals obstacle name")]
        [DropDownList(new string[] { "obstacleTypes", "stateList" }, new string[] { null, "name"}, new string[] { "MapManager", "StateManager" }, new string[] { "MapController", "this"}, new string[] { "Default" }, false, new string[] { "", ""})]
        public string name;
        [Tooltip("State priority order is set by position in list")]
        public List<CollisionProfile> statesAllowed;
    }
}