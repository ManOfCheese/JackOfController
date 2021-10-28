using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "SlideState", menuName = "States/SlideState" )]
public class SlideState : State {

	private SlideModule sm;

	public override void EnterState() {
		sm.jom.cam.transform.localPosition = new Vector3( sm.jom.cam.transform.localPosition.x, sm.slideCameHeight,
			sm.jom.cam.transform.localPosition.z );
		sm.jom.currentCamHeight = sm.slideCameHeight;
		sm.cc.height = sm.slidePlayerHeight;
		sm.cc.Move( Vector3.down * 200f );

		if ( sm.slideSpeed != 0f ) {
			sm.jom.currentSpeed = sm.slideSpeed;
		}
		else {
			sm.jom.currentSpeed = sm.spm.rSprintSpeed;
		}

		sm.slideStartVelocity = new Vector3( sm.cc.transform.forward.x, 0f, sm.cc.transform.forward.z ).normalized * sm.rSlideSpeed;
		sm.adjustedStartVelocity = sm.slideStartVelocity;
		sm.slideDir = sm.slideStartVelocity.normalized;
		sm.currentSlideSpeed = sm.rSlideSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		//_owner.joc.CameraLook();
		//sm.Slide();
		//_owner.joc.StickToGround();
		//_owner.joc.CheckGround();
		//sm.CheckSlope();
		if ( sm.groundSlopeAngle < sm.slopeAngleRange.x ) {
			sm.currentSlideSpeed -= sm.speedLoss * Time.deltaTime;
		}
		if ( sm.jom.currentSpeed < sm.joc.walkSpeed && sm.groundSlopeAngle < sm.slopeAngleRange.x ) {
			sm.jom.stateMachine.ChangeState( sm.jom.statesByName[ "GroundedState" ] );
		}
	}

	public override void ExitState() {
		sm.jom.cam.transform.localPosition = new Vector3( sm.jom.cam.transform.localPosition.x,
			sm.jom.camStartHeight, sm.jom.cam.transform.localPosition.z );
		sm.jom.currentCamHeight = sm.jom.camStartHeight;
		sm.cc.height = sm.jom.playerStartHeight;
		if ( sm.jom.currentSpeed >= sm.spm.rSprintSpeed ) {
			//sm.joc.StartSprint();
		}
		else {
			sm.jom.currentSpeed = sm.joc.walkSpeed;
		}
		sm.spm.sprinting = false;
	}

}
