using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "CheckSprint", menuName = "Functions/CheckSprint" )]
public class CheckSprint_Func : Function {

    private Core_Module core;
    private Sprint_Module spm;
    private Sprint_State sprintState;

    public override void Init() {
        spm = manager.modulesByName[ "SprintModule" ] as Sprint_Module;
        if ( spm == null ) Debug.LogError( "You cannot use the SprintCheck function without the Sprint Module." );

        core = spm.manager.core;

        sprintState = manager.statesByName[ "SprintState" ] as Sprint_State;
    }

    public override void ExecuteFunction() {
        if ( spm.sprintMode == SprintMode.HoldToSprint ) {
            if ( spm.sprintStart && manager.stateMachine.CurrentState != sprintState ) {
                spm.sprintStart = false;
                StartSprint();
            }
            else if ( spm.sprintEnd && manager.stateMachine.CurrentState == sprintState ) {
                spm.sprintEnd = false;
                EndSprint();
            }
        }
        else if ( spm.sprintMode == SprintMode.SinglePressSprint ) {
            if ( spm.sprintStart && manager.stateMachine.CurrentState != sprintState ) {
                spm.sprintStart = false;
                StartSprint();
            }
            else if ( !core.moving && manager.stateMachine.CurrentState == sprintState ) {
                EndSprint();
            }
        }
        else if ( spm.sprintMode == SprintMode.ToggleSprint ) {
            if ( spm.sprintToggle ) {
                bool newSprinting = !spm.sprinting;
                spm.sprintToggle = false;

                if ( newSprinting && manager.stateMachine.CurrentState != sprintState ) {
                    StartSprint();
                }
                else if ( manager.stateMachine.CurrentState == sprintState ) {
                    EndSprint();
                }
            }
        }
    }

    private void StartSprint() {
        manager.stateMachine.ChangeState( sprintState );
    }

    private void EndSprint() {
        if ( manager.core.grounded )
            manager.stateMachine.ChangeState( manager.statesByName[ "GroundedState" ] );
        else
            manager.stateMachine.ChangeState( manager.statesByName[ "AirborneState" ] );
    }
}
