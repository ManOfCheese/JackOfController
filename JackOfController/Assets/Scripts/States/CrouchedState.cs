using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "CrouchedState", menuName = "States/CrouchedState" )]
public class CrouchedState : State {

	private CrouchModule cm;

	public override void EnterState() {
		JackOfController joc = cm.joc;

		cm.jom.cam.transform.localPosition = new Vector3( cm.jom.cam.transform.localPosition.x, cm.crouchCamHeight, 
			cm.jom.cam.transform.localPosition.z );
		cm.jom.currentCamHeight = cm.crouchCamHeight;
		cm.jom.cc.height = cm.crouchPlayerHeight;
		cm.jom.cc.Move( Vector3.down );

		if ( cm.crouchSpeed != 0f )
			cm.jom.currentSpeed = cm.crouchSpeed;
		else
			cm.jom.currentSpeed = cm.joc.walkSpeed * cm.relativeCrouchSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		//_owner.joc.CameraLook();
		//_owner.joc.Walk();
		//_owner.joc.Gravity();
		//_owner.joc.CheckGround();
		if ( cm.crouchCanceled ) {
			if ( !cm.CheckCeiling() ) {
				cm.jom.stateMachine.ChangeState( cm.jom.statesByName[ "GroundedState" ] );
				cm.crouchCanceled = false;
			}
		}
	}

	public override void ExitState() {
		cm.jom.cam.transform.localPosition = new Vector3( cm.jom.cam.transform.localPosition.x,
			cm.jom.camStartHeight, cm.jom.cam.transform.localPosition.z );
		cm.jom.currentCamHeight = cm.jom.camStartHeight;
		cm.jom.cc.height = cm.jom.playerStartHeight;
		cm.jom.currentSpeed = cm.joc.walkSpeed;
	}
}
