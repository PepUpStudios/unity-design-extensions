using UnityEngine;
using System.Collections.Generic;

namespace State {
    public class StateManager : MonoBehaviour {

        public LogLevel logLevel;

        public string nameSpace;
        public List<StateVo> stateList;

        public Dictionary<string, IState> currentStateMap;
        public Dictionary<string, IState> initStateMap;
        public Dictionary<string, Dictionary<string, IState>> stateTransitionMap;

        void Start() {
            currentStateMap = new Dictionary<string, IState>();
            initStateMap = new Dictionary<string, IState>();
            stateTransitionMap = new Dictionary<string, Dictionary<string, IState>>();
            foreach (StateVo state in stateList) {
                if (!stateTransitionMap.ContainsKey(state.name)) {
                    stateTransitionMap.Add(state.name, new Dictionary<string, IState>());
                }
                List<StateTransition> transitionList = state.transitionList;
                IState loopStateObj = this.gameObject.GetComponent(nameSpace + "." + state.name) as IState;
                loopStateObj.Init(); //lifecycle method
                if (state.initial) { //if marked as initial state
                    AddCurrentState(loopStateObj);
                    initStateMap.Add(loopStateObj.GetName(), loopStateObj);
                }
                foreach (StateTransition stateTransition in transitionList) {
                    IState stateObj = null;
                    if (stateTransition.state != "EXIT") {
                        stateObj = this.gameObject.GetComponent(nameSpace + "." + stateTransition.state) as IState;
                    }
                    stateTransitionMap[state.name].Add(stateTransition.transition, stateObj);
                }
            }
        }

        public void OnEnable() {
            if (initStateMap == null) {
                return;
            }
            EnableInitStates();
        }

        protected void EnableInitStates() {
            foreach (string stateName in initStateMap.Keys) {
                AddCurrentState(initStateMap[stateName]);
            }
        }

        public void DoMultipleTransition(string transition) {
            IState currentState = null;
            Dictionary<string, IState> currStateTransitionMap = null;
            foreach (string stateName in currentStateMap.Keys) {
                currentState = currentStateMap[stateName];
                if (stateTransitionMap.ContainsKey(currentState.GetName())) {
                    currStateTransitionMap = stateTransitionMap[currentState.GetName()];
                    if (currStateTransitionMap.ContainsKey(transition)) {
                        IState targetState = currStateTransitionMap[transition];
                        if (targetState != null && targetState.IsTransitionAllowed(currentState)) {
                            CycleMethods(currentState, targetState);
                        } else { //EXIT SCENARIO - If a state wants to close(exit) another unrelated State
                            DisableCurrentState(currentState);
                        }
                    } 
                    #if UNITY_EDITOR
                    else {
                        DebugLog(currentState.GetName(), transition);
                    }
                    #endif
                }
            }
        }

        public void DoTransition<T>(string transition, params T[] objs) {
            bool flag = false;
            IState currentState = null;
            Dictionary<string, IState> currStateTransitionMap = null;
            foreach (string stateName in currentStateMap.Keys) {
                currentState = currentStateMap[stateName];
                if (stateTransitionMap.ContainsKey(currentState.GetName())) {
                    currStateTransitionMap = stateTransitionMap[currentState.GetName()];
                    if (currStateTransitionMap.ContainsKey(transition)) {
                        flag = true;
                        break;
                    }
                    #if UNITY_EDITOR
                    else {
                        DebugLog(currentState.GetName(), transition);
                    }
                    #endif
                }
            }
            if (flag) {
                IState targetState = currStateTransitionMap[transition];
                if (targetState != null && targetState.IsTransitionAllowed(currentState)) {
                    CycleMethods(currentState, targetState, objs);
                } else { //EXIT SCENARIO - If a state wants to close(exit) another unrelated State
                    DisableCurrentState(currentState);
                }
            }
        }

        public void DoTransition(string transition) {
            bool flag = false;
            IState currentState = null;
            Dictionary<string, IState> currStateTransitionMap = null;
            foreach (string stateName in currentStateMap.Keys) {
                currentState = currentStateMap[stateName];
                if (stateTransitionMap.ContainsKey(currentState.GetName())) {
                    currStateTransitionMap = stateTransitionMap[currentState.GetName()];
                    if (currStateTransitionMap.ContainsKey(transition)) {
                        flag = true;
                        break;
                    } 
                    #if UNITY_EDITOR
                    else {
                        DebugLog(currentState.GetName(), transition);
                    }
                    #endif
                }
            }
            if (flag) {
                IState targetState = currStateTransitionMap[transition];
                if (targetState != null && targetState.IsTransitionAllowed(currentState)) {
                    CycleMethods(currentState, targetState);
                } else if (targetState == null) { //EXIT SCENARIO - If a state wants to close(exit) another unrelated State
                    DisableCurrentState(currentState);
                }
            }
        }

        public void DoTransition(IState targetState) {
            bool flag = false;
            IState currentState = null;
            Dictionary<string, IState> currStateTransitionMap = null;
            foreach (string stateName in currentStateMap.Keys) {
                currentState = currentStateMap[stateName];
                if (stateTransitionMap.ContainsKey(currentState.GetName())) {
                    currStateTransitionMap = stateTransitionMap[currentState.GetName()];
                    if (currStateTransitionMap.ContainsValue(targetState)) {
                        flag = true;
                        break;
                    }
                    #if UNITY_EDITOR
                    else {
                        DebugLog(currentState.GetName(), targetState.GetName());
                    }
                    #endif
                }
            }
            if (flag) {
                if (targetState != null && targetState.IsTransitionAllowed(currentState)) {
                    CycleMethods(currentState, targetState);
                } else { //EXIT SCENARIO - If a state wants to close(exit) another unrelated State
                    DisableCurrentState(currentState);
                }
            }
        }

        protected void CycleMethods(IState currentState, IState targetState) {
            targetState.PreTransition(currentState); //lifecycle method
            currentState.PreEntryNewTransition(targetState); //lifecycle method
            AddCurrentState(targetState);
            currentState.PostEntryNewTransition(targetState); //lifecycle method
            targetState.PostTransition(currentState); //lifecycle method
        }

        protected void CycleMethods<T>(IState currentState, IState targetState, params T[] objs) {
            targetState.PreTransition(currentState, objs); //lifecycle method
            currentState.PreEntryNewTransition(targetState); //lifecycle method
            AddCurrentState(targetState);
            currentState.PostEntryNewTransition(targetState); //lifecycle method
            targetState.PostTransition(currentState, objs); //lifecycle method
        }

        public void AddCurrentState(IState state) {
            state.EnableState(true);
            if (!currentStateMap.ContainsKey(state.GetName())) {
                currentStateMap.Add(state.GetName(), state);
            }
        }

        public void DisableCurrentState(IState state) {
            state.EnableState(false);
            currentStateMap.Remove(state.GetName());
        }

        protected void DebugLog(string currentState, string targetState) {
            if (logLevel == LogLevel.Debug) {
                Debug.Log(currentState + " - " + targetState + " - NOT ALLOWED");
            }
        }
    }
}