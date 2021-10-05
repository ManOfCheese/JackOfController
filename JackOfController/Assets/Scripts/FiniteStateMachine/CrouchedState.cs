using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class CrouchedState : State<JackOfControllerManager> {

	public override void EnterState( JackOfControllerManager _owner ) {
		JackOfController bc = _owner.bc;
		CrouchModule cm = _owner.modules[ "CrouchModule" ] as CrouchModule;

		bc.cam.transform.localPosition = new Vector3( bc.cam.transform.localPosition.x, cm.crouchCamHeight, bc.cam.transform.localPosition.z );
		bc.cc.height = cm.crouchPlayerHeight;

		if ( cm.crouchSpeed != 0f )
			bc.currentSpeed = cm.crouchSpeed;
		else
			bc.currentSpeed = bc.speed * cm.relativeCrouchSpeed;
	}

	public override void UpdateState( JackOfControllerManager _owner ) {
		throw new System.NotImplementedException();
	}

	public override void ExitState( JackOfControllerManager _owner ) {
		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x, bc.camStartHeight, _owner.cam.transform.localPosition.z );
		_owner.cc.height = _owner.playerStartHeight;
		_owner.currentSpeed = _owner.speed;
	}
}
