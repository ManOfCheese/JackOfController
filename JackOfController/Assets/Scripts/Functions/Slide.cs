using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class Slide : Function {
    
    private SlideModule sm;

    public void Init( SlideModule _sm ) {
        this.sm = _sm;
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
		if ( sm.adjustedStartVelocity != Vector3.zero ) {
			float magnitude = sm.adjustedStartVelocity.magnitude;
			magnitude -= sm.speedLoss * Time.deltaTime;
			sm.adjustedStartVelocity = sm.adjustedStartVelocity.normalized * magnitude;
		}

		//If we have changed directions to drasticalyl since we started our slide, exit it.
		//This is so that if you slide up hill you stop when you start to fall backwards.
		float directionDiffRad = Mathf.Acos( Vector2.Dot( new Vector2( sm.slideDir.x, sm.slideDir.z ).normalized,
			new Vector2( sm.slideStartVelocity.x, sm.slideStartVelocity.z ).normalized ) );
		float directionDiffAngle = directionDiffRad * Mathf.Rad2Deg;

		if ( directionDiffAngle > 90f )
			sm.jom.stateMachine.ChangeState( sm.jom.statesByName[ "GroundedState" ] );

		//Finally combine the slope direction * the slide speed with our initial slide velocity to get that smooth transition.
		Vector3 slopeVector = sm.slopeDir * sm.rSlideSpeed;
		if ( sm.slopeBasedSpeed )
			slopeVector *= slopeMod;
		Vector3 finalSlideVector = slopeVector + sm.adjustedStartVelocity;
		sm.jom.currentSpeed = finalSlideVector.magnitude;

		sm.jom.cc.Move( finalSlideVector * Time.deltaTime );
		sm.slideDir = finalSlideVector.normalized;
	}
}
