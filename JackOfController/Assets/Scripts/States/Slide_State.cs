using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "Slide_State", menuName = "States/Slide_State" )]
public class Slide_State : State {

	private Slide_Module sm;
	private Crouch_Module cm;
	private Camera cam;
	private CharacterController cc;
	private Core_Module core;

	public override void Init() {
		sm = manager.modulesByName[ "SlideModule" ] as Slide_Module;
		cam = manager.cam;
		cc = manager.cc;
		core = manager.core;

		if ( manager.modulesByName[ "CrouchModule" ] )
			cm = manager.modulesByName[ "CrouchModule" ] as Crouch_Module;
	}

	public override void EnterState() {
		cam.transform.localPosition = new Vector3( cam.transform.localPosition.x, sm.slideCameHeight, cam.transform.localPosition.z );
		manager.currentCamHeight = sm.slideCameHeight;
		cc.height = sm.slidePlayerHeight;
		cc.Move( Vector3.down * 200f );

		manager.currentSpeed = sm.slideSpeed;

		sm.initialSlideVelocity = new Vector3( cc.transform.forward.x, 0f, cc.transform.forward.z ).normalized * sm.slideSpeed;
		sm.currentSlideVelocity = sm.initialSlideVelocity;
		sm.slideDir = sm.initialSlideVelocity.normalized;
		sm.currentSlideSpeed = sm.slideSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		if ( sm.groundSlopeAngle < sm.slopeAngleRange.x )
			sm.currentSlideSpeed -= sm.speedLoss * Time.deltaTime;

		//If we are going too slow switch to another state.
		if ( cm ) {
			if ( manager.currentSpeed < cm.crouchSpeed ) {
				manager.stateMachine.ChangeState( manager.statesByName[ "CrouchState" ] );
			}
		}
		else {
			if ( manager.currentSpeed < core.walkSpeed && sm.groundSlopeAngle < sm.slopeAngleRange.x ) {
				manager.stateMachine.ChangeState( sm.groundedState );
			}
		}
	}

	public override void ExitState() {
		cam.transform.localPosition = new Vector3( cam.transform.localPosition.x, manager.camStartHeight, cam.transform.localPosition.z );
		manager.currentCamHeight = sm.manager.camStartHeight;
		cc.height = sm.manager.playerStartHeight;
	}

}
