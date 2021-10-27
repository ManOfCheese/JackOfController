using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "SlideState", menuName = "States/SlideState" )]
public class SlideState : State {

	public override void EnterState() {
		JackOfController joc = _owner.joc;
		SlideModule sm = _owner.modulesByName[ "SlideModule" ] as SlideModule;

		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x, sm.slideCameHeight,
			_owner.cam.transform.localPosition.z );
		joc.currentCamHeight = sm.slideCameHeight;
		_owner.cc.height = sm.slidePlayerHeight;
		_owner.cc.Move( Vector3.down * 200f );

		if ( sm.slideSpeed != 0f ) {
			_owner.joc.currentSpeed = sm.slideSpeed;
		}
		else {
			if ( _owner.joc.sprintSpeed != 0f )
				_owner.joc.currentSpeed = _owner.joc.sprintSpeed * sm.relativeSlideSpeed;
			else
				_owner.joc.currentSpeed = _owner.joc.walkSpeed * _owner.joc.relativeSprintSpeed * sm.relativeSlideSpeed;
		}

		sm.slideStartVelocity = new Vector3( _owner.cc.transform.forward.x, 0f, _owner.cc.transform.forward.z ).normalized * sm.rSlideSpeed;
		sm.adjustedStartVelocity = sm.slideStartVelocity;
		sm.slideDir = sm.slideStartVelocity.normalized;
		sm.currentSlideSpeed = sm.rSlideSpeed;
	}

	public override void UpdateState() {
		SlideModule sm = _owner.modulesByName[ "SlideModule" ] as SlideModule;

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
		if ( _owner.joc.currentSpeed < _owner.joc.walkSpeed && sm.groundSlopeAngle < sm.minSlopeAngle ) {
			_owner.stateMachine.ChangeState( _owner.statesByName[ "GroundedState" ] );
		}
	}

	public override void ExitState() {
		Debug.Log( "Exiting Slide" );
		_owner.cam.transform.localPosition = new Vector3( _owner.cam.transform.localPosition.x,
			_owner.joc.camStartHeight, _owner.cam.transform.localPosition.z );
		_owner.joc.currentCamHeight = _owner.joc.camStartHeight;
		_owner.cc.height = _owner.joc.playerStartHeight;
		if ( _owner.joc.currentSpeed >= _owner.joc.rSprintSpeed ) {
			_owner.joc.StartSprint();
		}
		else {
			_owner.joc.currentSpeed = _owner.joc.walkSpeed;
		}
		_owner.joc.sprinting = false;
	}

}
