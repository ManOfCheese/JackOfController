using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Slide_System", menuName = "Systems/SlideSystem" )]
public class SlideSystem : ComponentSystem {

	public SlideModule sm;

	public override void Init() {
		if ( sm.slideSpeed != 0f )
			sm.rSlideSpeed = sm.slideSpeed;
		else
			sm.rSlideSpeed = sm.spm.rSprintSpeed * sm.relativeSlideSpeed;
	}

	public override void OnUpdate() {
		if ( sm.jom.currentSpeed < sm.jom.joc.walkSpeed ) {
			sm.jom.stateMachine.ChangeState( sm.jom.statesByName[ "GroundedState" ] );
		}
		//sm.CheckSlope();
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.transform.forward * 20f, Color.blue );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slopeDir.normalized * 20f, Color.white );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slideDir.normalized * 20f, Color.red );
	}

}
