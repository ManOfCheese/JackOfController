using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : Function {

    private JackOfController joc;

    public void Init( JackOfController _joc ) {
        this.joc = _joc;
    }

    public override void ExecuteFunction() {
        bool newGrounded;

        float radius = joc.jom.cc.height / 4f;
        newGrounded = Physics.CheckSphere( new Vector3( joc.jom.cc.transform.position.x, joc.jom.cc.transform.position.y - radius, 
            joc.jom.cc.transform.position.z ), joc.groundDistance, joc.groundMask );

        if ( newGrounded != joc.grounded ) {
            if ( newGrounded && joc.jom.stateMachine.CurrentState != joc.jom.statesByName[ "GroundedState" ] ) {
                joc.jom.stateMachine.ChangeState( joc.jom.statesByName[ "GroundedState" ] );
            }
            if ( !newGrounded && joc.jom.stateMachine.CurrentState != joc.jom.statesByName[ "AirborneState" ] )
                joc.jom.stateMachine.ChangeState( joc.jom.statesByName[ "AirborneState" ] );
        }

        joc.grounded = newGrounded;
    }

}
