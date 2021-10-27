using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "CrouchedState", menuName = "States/CrouchedState" )]
public class CrouchedState : State {

	public override void EnterState() {
		JackOfController joc = _owner.joc;
		CrouchModule cm = _owner.modulesByName[ "CrouchModule" ] as CrouchModule;

		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x, cm.crouchCamHeight, 
			_owner.cam.transform.localPosition.z );
		_owner.currentCamHeight = cm.crouchCamHeight;
		_owner.cc.height = cm.crouchPlayerHeight;
		_owner.cc.Move( Vector3.down );

		if ( cm.crouchSpeed != 0f )
			_owner.currentSpeed = cm.crouchSpeed;
		else
			_owner.currentSpeed = _owner.joc.walkSpeed * cm.relativeCrouchSpeed;
	}

	public override void UpdateState() {
		CrouchModule cm = _owner.modulesByName[ "CrouchModule" ] as CrouchModule;

		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		//_owner.joc.CameraLook();
		//_owner.joc.Walk();
		//_owner.joc.Gravity();
		//_owner.joc.CheckGround();
		if ( cm.crouchCanceled ) {
			if ( !cm.CheckCeiling() ) {
				_owner.stateMachine.ChangeState( _owner.statesByName[ "GroundedState" ] );
				cm.crouchCanceled = false;
			}
		}
	}

	public override void ExitState() {
		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x,
			_owner.joc.camStartHeight, _owner.cam.transform.localPosition.z );
		_owner.joc.currentCamHeight = _owner.joc.camStartHeight;
		_owner.cc.height = _owner.joc.playerStartHeight;
		_owner.joc.currentSpeed = _owner.joc.walkSpeed;
	}
}
