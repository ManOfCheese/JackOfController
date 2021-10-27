using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public class FiniteStateMachine {

        public State CurrentState {
            get; private set;
        } 

        public FiniteStateMachine() {
            CurrentState = null;
        }

        public void ChangeState( State _newState ) {
            if ( CurrentState != null ) {
                CurrentState.ExitState();
            }
            //Debug.Log( "Switched from " + CurrentState + " to " + _newState );
            CurrentState = _newState;
            CurrentState.EnterState();
        }

        public void Update() {
            if ( CurrentState != null ) {
                CurrentState.UpdateState();
            }
        }
    }

    public abstract class State : ScriptableObject {
        public string stateName;
        public Function[] functionsToUpdate;
        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void UpdateState();
    }
}