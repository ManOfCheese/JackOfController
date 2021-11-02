using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SlideMode {
	ToggleSlide,
	HoldToSlide,
	SinglePressSlide
}

public class Slide_Module : Module {

	public Slide_System system;

	[Header( "Slide Settings" )]
	[Tooltip( "When true you toggle between sliding and not sliding." )]
	public SlideMode slideMode = SlideMode.HoldToSlide;
	[Tooltip( "When true your speed is modified based on the angle of the slope." )]
	public bool slopeBasedSpeed = true;
	[Tooltip( "Speed in units per second." )]
	public float absSlideSpeed = 0f;
	[Tooltip( "Speed in units per second." )]
	public float relativeSlideSpeed = 2f;
	[Tooltip( "How fast you lose your initial velocity boost from sliding." )]
	public float speedLoss = 1f;
	[Tooltip( "The height of your camera while sliding." )]
	public float slideCameHeight = 0.5f;
	[Tooltip( "The height of the player while sliding." )]
	public float slidePlayerHeight = 0.8f;
	[Tooltip( "Minimum slope need to be able to slide." )]
	public Vector2 slopeAngleRange = new Vector2( 0f, 60f );
	[Tooltip( "The speed multiplier applied depening on the steepness of the slope" )]
	public Vector2 slopeBoostRange = new Vector2( 1f, 2f );

	[Header( "SlopeCheck Settings" )]
	public bool showDebug;
	public float startDistanceFromBottom = 0.2f;   // Should probably be higher than skin width
	public float sphereCastRadius = 0.25f;
	public float sphereCastDistance = 0.75f;
	public float raycastLength = 0.75f;
	public LayerMask castingMask;
	public Vector3 rayOriginOffset1 = new Vector3( -0.2f, 0f, 0.16f );
	public Vector3 rayOriginOffset2 = new Vector3( 0.2f, 0f, -0.16f );
	public Vector3 slopeCheckOrigin = Vector3.zero;

	[Header( "Debug" )]
	//The adjusted velocity is a copy of slideStartVelocity that loses speed over time. 
	//We do this because we need start velocity to comapre to the slide direciton.
	[ReadOnly] public bool sliding;
	[ReadOnly] public float groundSlopeAngle;
	[ReadOnly] public Vector3 currentSlideVelocity;
	[ReadOnly] public Vector3 slideDir;
	[ReadOnly] public Vector3 slopeDir;

	[HideInInspector] public float slideSpeed;
	[HideInInspector] public float currentSlideSpeed;
	[HideInInspector] public Vector3 initialSlideVelocity;
	[HideInInspector] public Sprint_Module sprintModule;
	[HideInInspector] public Core_Module core;
	[HideInInspector] public Airborne_State airborneState;
	[HideInInspector] public Grounded_State groundedState;
	[HideInInspector] public Slide_State slideState;

	protected override void Awake() {
		system.sm = this;
		if ( moduleName == "" ) moduleName = "SlideModule";
	}

	public void OnSlide( InputAction.CallbackContext value ) {
		if ( manager.stateMachine.CurrentState != airborneState ) {
			if ( slideMode == SlideMode.SinglePressSlide ) {
				if ( sprintModule.sprinting && manager.stateMachine.CurrentState != slideState )
					manager.stateMachine.ChangeState( slideState );
			}
			else if ( slideMode == SlideMode.HoldToSlide ) {
				if ( value.started && sprintModule.sprinting && manager.stateMachine.CurrentState != slideState )
					manager.stateMachine.ChangeState( slideState );
				if ( value.canceled && manager.stateMachine.CurrentState == slideState )
					EndSlide();
			}
			else if ( slideMode == SlideMode.ToggleSlide ) {
				sliding = !sliding;
				if ( sliding && sprintModule.sprinting && manager.stateMachine.CurrentState != slideState )
					manager.stateMachine.ChangeState( slideState );
				else if ( !sliding && manager.stateMachine.CurrentState == slideState )
					EndSlide();
			}
		}
	}

	private void EndSlide() {
		if ( manager.core.grounded ) {
			if ( !core.CheckCeiling() ) {
				if ( manager.statesByName[ "SprintState" ] && currentSlideSpeed >= sprintModule.sprintSpeed )
					manager.stateMachine.ChangeState( manager.statesByName[ "SprintState" ] );
				else
					manager.stateMachine.ChangeState( groundedState );
			}
			else if ( manager.statesByName[ "CrouchState" ] ) {
				Crouch_Module cm = manager.modulesByName[ "CrouchModule" ] as Crouch_Module;
				manager.stateMachine.ChangeState( manager.statesByName[ "CrouchState" ] );
				cm.crouchCanceled = true;
			}
		}
		else
			manager.stateMachine.ChangeState( airborneState );
	}

	private void OnDrawGizmos() {

		Gizmos.color = Color.yellow;
		if ( manager != null ) {
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( slideDir.normalized * jocManager.joc.currentSpeed ) );
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( groundSlopeDir.normalized * jocManager.joc.currentSpeed ) );

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine( manager.cc.transform.position, manager.cc.transform.position +
				( new Vector3( slopeDir.normalized.x, 0f, slopeDir.normalized.z ) * 20f ) );

			Gizmos.color = Color.red;
			Gizmos.DrawLine( manager.cc.transform.position, manager.cc.transform.position +
				( new Vector3( manager.cc.transform.forward.normalized.x, 0f, manager.cc.transform.forward.normalized.z ) * 20f ) );
		}
	}
}
