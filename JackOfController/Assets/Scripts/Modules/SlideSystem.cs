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
			sm.rSlideSpeed = sm.jocManager.joc.rSprintSpeed * sm.relativeSlideSpeed;
	}

	public override void OnUpdate() {
		if ( sm.jocManager.joc.currentSpeed < sm.jocManager.joc.speed ) {
			sm.jocManager.stateMachine.ChangeState( sm.jocManager.statesByName[ "GroundedState" ] );
		}
		sm.CheckSlope();
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.transform.forward * 20f, Color.blue );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slopeDir.normalized * 20f, Color.white );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slideDir.normalized * 20f, Color.red );
	}

}
