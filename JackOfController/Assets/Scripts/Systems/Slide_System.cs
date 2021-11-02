using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Slide_System", menuName = "Systems/Slide_System" )]
public class Slide_System : ComponentSystem {

	public Slide_Module sm;

	public override void Init() {
		if ( sm.manager.modulesByName[ "SprintModule" ] )
			sm.sprintModule = sm.manager.modulesByName[ "SprintModule" ] as Sprint_Module;

		sm.core = sm.manager.core;
		sm.groundedState = sm.manager.statesByName[ "GroundedState" ] as Grounded_State;
		sm.slideState = sm.manager.statesByName[ "SlideState" ] as Slide_State;

		if ( sm.absSlideSpeed != 0f ) {
			sm.slideSpeed = sm.absSlideSpeed;
		}
		else {
			if ( sm.sprintModule )
				sm.slideSpeed = sm.sprintModule.sprintSpeed * sm.relativeSlideSpeed;
			else
				sm.slideSpeed = sm.core.walkSpeed * sm.relativeSlideSpeed;
		}
	}

	public override void OnUpdate() {
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.transform.forward * 20f, Color.blue );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slopeDir.normalized * 20f, Color.white );
		Debug.DrawLine( sm.transform.position, sm.transform.position + sm.slideDir.normalized * 20f, Color.red );
	}

}
