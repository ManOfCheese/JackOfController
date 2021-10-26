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
	public float minSlopeAngle = 0f;
	[Tooltip( "Maximum slope you can slide off." )]
	public float maxSlopeAngle = 60f;
	[Tooltip( "How fast you lose your initial velocity boost from sliding." )]
	public float speedLoss = 1f;
	[Tooltip( "The height of your camera while sliding." )]
	public float slideCameHeight = 0.5f;
	[Tooltip( "The height of the player while sliding." )]
	public float slidePlayerHeight = 0.8f;
	public float slopeBoostMin = 1f;
	public float slopeBoostMax = 2f;

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
		state = SlideState.Instance;
		state.stateName = "SlideState";
	}

	public void OnSlide( InputAction.CallbackContext value ) {
		if ( slideMode == SlideMode.SinglePressSlide ) {
			if ( jocManager.joc.sprinting && jocManager.stateMachine.CurrentState != jocManager.statesByName[ "SlideState" ] )
				jocManager.stateMachine.ChangeState( jocManager.statesByName[ "SlideState" ] );
		}
		else if ( slideMode == SlideMode.HoldToSlide ) {
			if ( value.started )
				jocManager.stateMachine.ChangeState( jocManager.statesByName[ "SlideState" ] );
			if ( value.canceled )
				jocManager.stateMachine.ChangeState( jocManager.statesByName[ "GroundedState" ] );
		}
		else if ( slideMode == SlideMode.ToggleSlide ) {
			sliding = !sliding;
			if ( sliding )
				jocManager.stateMachine.ChangeState( jocManager.statesByName[ "SlideState" ] );
			else
				jocManager.stateMachine.ChangeState( jocManager.statesByName[ "GroundedState" ] );
		}
	}

	public void Slide() {
		float slopeMod = 1f;
		if ( slopeBasedSpeed ) {
			float clampedAngle = Mathf.Clamp( groundSlopeAngle, minSlopeAngle, maxSlopeAngle );
			slopeMod = Map( clampedAngle, minSlopeAngle, maxSlopeAngle, slopeBoostMin, slopeBoostMax );
		}

		float slopeDiffRad = Mathf.Acos( Vector2.Dot( new Vector2( slideDir.x, slideDir.z ).normalized,
			new Vector2( slopeDir.x, slopeDir.z ).normalized ) );
		float slopeDiffAngle = slopeDiffRad * Mathf.Rad2Deg;
		float slopeDiffMod = Map( slopeDiffAngle, 0f, 180f, 1f, 0f );
		slopeMod *= slopeDiffMod;

		//Have our initial velocity decrease over time to have it slowly lerp towards to the slope direction.
		if ( adjustedStartVelocity != Vector3.zero ) {
			float magnitude = adjustedStartVelocity.magnitude;
			magnitude -= speedLoss * Time.deltaTime;
			adjustedStartVelocity = adjustedStartVelocity.normalized * magnitude;
		}

		//If we have changed directions to drasticalyl since we started our slide, exit it.
		//This is so that if you slide up hill you stop when you start to fall backwards.
		float directionDiffRad = Mathf.Acos( Vector2.Dot( new Vector2( slideDir.x, slideDir.z ).normalized, 
			new Vector2( slideStartVelocity.x, slideStartVelocity.z ).normalized ) );
		float directionDiffAngle = directionDiffRad * Mathf.Rad2Deg;

		if ( directionDiffAngle > 90f )
			jocManager.stateMachine.ChangeState( jocManager.statesByName[ "GroundedState" ] );

		//Finally combine the slope direction * the slide speed with our initial slide velocity to get that smooth transition.
		Vector3 slopeVector = slopeDir * rSlideSpeed;
		if ( slopeBasedSpeed )
			slopeVector *= slopeMod;
		Vector3 finalSlideVector = slopeVector + adjustedStartVelocity;
		jocManager.joc.currentSpeed = finalSlideVector.magnitude;

		jocManager.cc.Move( finalSlideVector * Time.deltaTime );
		slideDir = finalSlideVector.normalized;
	}

	public void CheckSlope() {
		slopeCheckOrigin = new Vector3( transform.position.x, transform.position.y - ( jocManager.cc.height / 2 ) + startDistanceFromBottom,
			transform.position.z );
		RaycastHit hit;
		if ( Physics.SphereCast( slopeCheckOrigin, sphereCastRadius, Vector3.down, out hit, sphereCastDistance, castingMask ) ) {
			groundSlopeAngle = Vector3.Angle( hit.normal, Vector3.up );
			Vector3 temp = Vector3.Cross( hit.normal, Vector3.down );
			slopeDir = Vector3.Cross( temp, hit.normal );
		}

		RaycastHit slopeHit1;
		RaycastHit slopeHit2;

		if ( Physics.Raycast( slopeCheckOrigin + rayOriginOffset1, Vector3.down, out slopeHit1, raycastLength ) ) {

			if ( showDebug ) { Debug.DrawLine( slopeCheckOrigin + rayOriginOffset1, slopeHit1.point, Color.red ); }

			float angleOne = Vector3.Angle( slopeHit1.normal, Vector3.up );

			if ( Physics.Raycast( slopeCheckOrigin + rayOriginOffset2, Vector3.down, out slopeHit2, raycastLength ) ) {

				if ( showDebug ) { Debug.DrawLine( slopeCheckOrigin + rayOriginOffset2, slopeHit2.point, Color.red ); }

				float angleTwo = Vector3.Angle( slopeHit2.normal, Vector3.up );
				List<float> tempArray = new List<float>() { groundSlopeAngle, angleOne, angleTwo };
				tempArray.Sort();
				groundSlopeAngle = tempArray[ 1 ];
			}
			else {
				float average = ( groundSlopeAngle + angleOne ) / 2;
				groundSlopeAngle = average;
			}
		}
	}

	private void OnDrawGizmos() {

		Gizmos.color = Color.yellow;
		if ( jocManager != null ) {
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( slideDir.normalized * jocManager.joc.currentSpeed ) );
			//Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
			//	( groundSlopeDir.normalized * jocManager.joc.currentSpeed ) );

			Gizmos.color = Color.yellow;
			Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
				( new Vector3( slopeDir.normalized.x, 0f, slopeDir.normalized.z ) * 20f ) );

			Gizmos.color = Color.red;
			Gizmos.DrawLine( jocManager.cc.transform.position, jocManager.cc.transform.position +
				( new Vector3( jocManager.cc.transform.forward.normalized.x, 0f, jocManager.cc.transform.forward.normalized.z ) * 20f ) );
		}
	}

	public float Map( float value, float inputFrom, float inputTo, float outputFrom, float outputTo ) {
		return ( value - inputFrom ) / ( inputTo - inputFrom ) * ( outputTo - outputFrom ) + outputFrom;
	}

}
