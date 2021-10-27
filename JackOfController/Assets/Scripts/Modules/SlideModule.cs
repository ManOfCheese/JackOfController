using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SlideMode {
	ToggleSlide,
	HoldToSlide,
	SinglePressSlide
}

public class SlideModule : Module {

	public SlideSystem system;

	[Header( "Slide Settings" )]
	[Tooltip( "When true you toggle between sliding and not sliding." )]
	public SlideMode slideMode = SlideMode.HoldToSlide;
	[Tooltip( "When true your speed is modified based on the angle of the slope." )]
	public bool slopeBasedSpeed = true;
	[Tooltip( "Speed in units per second." )]
	public float slideSpeed = 0f;
	[Tooltip( "Speed in units per second." )]
	public float relativeSlideSpeed = 2f;
	[Tooltip( "Minimum slope need to be able to slide." )]
	public Vector2 slopeAngleRange = new Vector2( 0f, 60f );
	[Tooltip( "How fast you lose your initial velocity boost from sliding." )]
	public float speedLoss = 1f;
	[Tooltip( "The height of your camera while sliding." )]
	public float slideCameHeight = 0.5f;
	[Tooltip( "The height of the player while sliding." )]
	public float slidePlayerHeight = 0.8f;
	[Tooltip( "The speed multiplier applied depening on the steepness of the slope" )]
	public Vector2 slopeBoostRange = new Vector2( 1f, 2f );

	[Header( "SlopeCheck Settings" )]
	public bool showDebug;
	public LayerMask castingMask;
	public float startDistanceFromBottom = 0.2f;   // Should probably be higher than skin width
	public float sphereCastRadius = 0.25f;
	public float sphereCastDistance = 0.75f;
	public float raycastLength = 0.75f;
	public Vector3 rayOriginOffset1 = new Vector3( -0.2f, 0f, 0.16f );
	public Vector3 rayOriginOffset2 = new Vector3( 0.2f, 0f, -0.16f );
	public Vector3 slopeCheckOrigin = Vector3.zero;

	[HideInInspector] public JackOfController joc;
	[HideInInspector] public float rSlideSpeed;
	[HideInInspector] public float currentSlideSpeed;
	[HideInInspector] public Vector3 slideStartVelocity;

	//The adjusted velocity is a copy of slideStartVelocity that loses speed over time. 
	//We do this because we need start velocity to comapre to the slide direciton.
	[ReadOnly] public Vector3 adjustedStartVelocity;
	[ReadOnly] public Vector3 slideDir;
	[ReadOnly] public Vector3 slopeDir;
	[ReadOnly] public float groundSlopeAngle;
	[ReadOnly] public bool sliding;

	protected override void Awake() {
		system.sm = this;
	}

	public void OnSlide( InputAction.CallbackContext value ) {
		if ( slideMode == SlideMode.SinglePressSlide ) {
			if ( jom.joc.sprinting && jom.stateMachine.CurrentState != jom.statesByName[ "SlideState" ] )
				jom.stateMachine.ChangeState( jom.statesByName[ "SlideState" ] );
		}
		else if ( slideMode == SlideMode.HoldToSlide ) {
			if ( value.started )
				jom.stateMachine.ChangeState( jom.statesByName[ "SlideState" ] );
			if ( value.canceled )
				jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
		}
		else if ( slideMode == SlideMode.ToggleSlide ) {
			sliding = !sliding;
			if ( sliding )
				jom.stateMachine.ChangeState( jom.statesByName[ "SlideState" ] );
			else
				jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
		}
	}

	private void OnDrawGizmos() {

		Gizmos.color = Color.yellow;
		if ( jom != null ) {
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( slideDir.normalized * jocManager.joc.currentSpeed ) );
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( groundSlopeDir.normalized * jocManager.joc.currentSpeed ) );

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine( jom.cc.transform.position, jom.cc.transform.position +
				( new Vector3( slopeDir.normalized.x, 0f, slopeDir.normalized.z ) * 20f ) );

			Gizmos.color = Color.red;
			Gizmos.DrawLine( jom.cc.transform.position, jom.cc.transform.position +
				( new Vector3( jom.cc.transform.forward.normalized.x, 0f, jom.cc.transform.forward.normalized.z ) * 20f ) );
		}
	}
}
