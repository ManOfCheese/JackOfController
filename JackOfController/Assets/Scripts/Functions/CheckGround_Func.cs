using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "CheckGround", menuName = "Functions/CheckGround" )]
public class CheckGround_Func : Function {

    private Core_Module core;
    private Jump_Module jm;
    private Airborne_State airborneState;
    private Grounded_State groundedState;

    public override void Init() {
        core = manager.core;
        if ( manager.modulesByName[ "JumpModule" ] ) {
            jm = manager.modulesByName[ "JumpModule" ] as Jump_Module;
        }
        airborneState = manager.statesByName[ "AirborneState" ] as Airborne_State;
        groundedState = manager.statesByName[ "GroundedState" ] as Grounded_State;
    }

    public override void ExecuteFunction() {
        bool newGrounded;

        float radius = core.manager.cc.height / 4f;
        newGrounded = Physics.CheckSphere( new Vector3( core.manager.cc.transform.position.x, core.manager.cc.transform.position.y - radius, 
            core.manager.cc.transform.position.z ), core.groundDistance, core.groundMask );

        if ( newGrounded != core.grounded ) {
            if ( newGrounded && core.manager.stateMachine.CurrentState != groundedState ) {
                core.manager.stateMachine.ChangeState( groundedState );
                jm.velocityOnJump = Vector3.zero;
                jm.jumpCount = 0;
            }
            if ( !newGrounded && core.manager.stateMachine.CurrentState != airborneState ) {
                core.manager.stateMachine.ChangeState( airborneState );
            }
        }

        core.grounded = newGrounded;
    }

}
