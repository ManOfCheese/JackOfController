using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "Crouch_State", menuName = "States/Crouch_State" )]
public class Crouch_State : State {

	private Crouch_Module cm;
	private Camera cam;
	private CharacterController cc;
	private Core_Module core;

	public override void Init() {
		cm = manager.modulesByName[ "CrouchModule" ] as Crouch_Module;
		cam = manager.cam;
		cc = manager.cc;
		core = manager.core;
	}

	public override void EnterState() {
		cam.transform.localPosition = new Vector3( cam.transform.localPosition.x, cm.crouchCamHeight, 
			cam.transform.localPosition.z );
		manager.currentCamHeight = cm.crouchCamHeight;
		cc.height = cm.crouchPlayerHeight;
		cc.Move( Vector3.down );
		manager.currentSpeed = cm.crouchSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		if ( cm.crouchCanceled ) {
			if ( !core.CheckCeiling() ) {
				manager.stateMachine.ChangeState( cm.groundedState );
				cm.crouchCanceled = false;
			}
		}
	}

	public override void ExitState() {
		cm.manager.cam.transform.localPosition = new Vector3( cm.manager.cam.transform.localPosition.x,
			cm.manager.camStartHeight, cam.transform.localPosition.z );
		manager.currentCamHeight = manager.camStartHeight;
		cc.height = manager.playerStartHeight;
		manager.currentSpeed = core.walkSpeed;
	}
}
