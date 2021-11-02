using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Crouch_System", menuName = "Systems/Crouch_System" )]
public class Crouch_System : ComponentSystem {

	public Crouch_Module cm;

	public override void Init() {
		cm.sprintModule = cm.manager.modulesByName[ "SprintModule" ] as Sprint_Module;
		cm.crouchState = cm.manager.statesByName[ "CrouchState" ] as Crouch_State;
		cm.airborneState = cm.manager.statesByName[ "AirborneState" ] as Airborne_State;
		cm.groundedState = cm.manager.statesByName[ "GroundedState" ] as Grounded_State;

		if ( cm.absCrouchSpeed != 0f )
			cm.crouchSpeed = cm.absCrouchSpeed;
		else
			cm.crouchSpeed = cm.manager.core.walkSpeed * cm.relativeCrouchSpeed;
	}

	public override void OnUpdate() {
	}

}
