using System.Collections.Generic;

namespace State {
    [System.Serializable]
    public class StateVo {

        public string name;
        public bool initial;
        public List<StateTransition> transitionList;
    }
}