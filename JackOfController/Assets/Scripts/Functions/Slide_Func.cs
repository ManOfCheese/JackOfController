using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

[CreateAssetMenu( fileName = "Slide", menuName = "Functions/Slide" )]
public class Slide_Func : Function {
    
    private Slide_Module sm;

    public override void Init() {
        sm = manager.modulesByName[ "SlideModule" ] as Slide_Module;
		if ( sm == null ) Debug.LogError( "You cannot use the Slide function without the Slide Module." );
	}

	public override void ExecuteFunction() {
		float slopeMod = 1f;
		if ( sm.slopeBasedSpeed ) {
			float clampedAngle = Mathf.Clamp( sm.groundSlopeAngle, sm.slopeAngleRange.x, sm.slopeAngleRange.y );
			slopeMod = CustomMath.CustomMath.Map( clampedAngle, sm.slopeAngleRange.x, sm.slopeAngleRange.y, sm.slopeBoostRange.x, sm.slopeBoostRange.y );
		}

		float slopeDiffRad = Mathf.Acos( Vector2.Dot( new Vector2( sm.slideDir.x, sm.slideDir.z ).normalized,
			new Vector2( sm.slopeDir.x, sm.slopeDir.z ).normalized ) );
		float slopeDiffAngle = slopeDiffRad * Mathf.Rad2Deg;
		float slopeDiffMod = CustomMath.CustomMath.Map( slopeDiffAngle, 0f, 180f, 1f, 0f );
		slopeMod *= slopeDiffMod;

		//Have our initial velocity decrease over time to have it slowly lerp towards to the slope direction.
		if ( sm.currentSlideVelocity != Vector3.zero ) {
			float magnitude = sm.currentSlideVelocity.magnitude;
			magnitude -= sm.speedLoss * Time.deltaTime;
			sm.currentSlideVelocity = sm.currentSlideVelocity.normalized * magnitude;
		}

		//If we have changed directions to drastically since we started our slide, exit it.
		//This is so that if you slide up hill you stop when you start to fall backwards.
		float directionDiffRad = Mathf.Acos( Vector2.Dot( new Vector2( sm.slideDir.x, sm.slideDir.z ).normalized,
			new Vector2( sm.initialSlideVelocity.x, sm.initialSlideVelocity.z ).normalized ) );
		float directionDiffAngle = directionDiffRad * Mathf.Rad2Deg;

		if ( directionDiffAngle > 90f )
			sm.manager.stateMachine.ChangeState( sm.manager.statesByName[ "GroundedState" ] );

		//Finally combine the slope direction * the slide speed with our initial slide velocity to get that smooth transition.
		Vector3 slopeVector = sm.slopeDir * sm.slideSpeed;
		if ( sm.slopeBasedSpeed )
			slopeVector *= slopeMod;
		Vector3 finalSlideVector = slopeVector + sm.currentSlideVelocity;
		if ( finalSlideVector.magnitude > Mathf.Max( slopeVector.magnitude, sm.currentSlideVelocity.magnitude ) )
			finalSlideVector = finalSlideVector.normalized * Mathf.Max( slopeVector.magnitude, sm.currentSlideVelocity.magnitude );
		
		if ( sm.manager.currentState == manager.statesByName[ "SlideState" ].stateName )
			sm.manager.currentSpeed = finalSlideVector.magnitude;
		Debug.Log( "Zeg makker " + manager.currentSpeed );

		sm.manager.cc.Move( finalSlideVector * Time.deltaTime );
		sm.slideDir = finalSlideVector.normalized;
	}
}
