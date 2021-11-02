using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Sprint_System", menuName = "Systems/Sprint_System" )]
public class Sprint_System : ComponentSystem {

	public Sprint_Module sm;

	public override void Init() {
		if ( sm.absSprintSpeed != 0f )
			sm.sprintSpeed = sm.absSprintSpeed;
		else
			sm.sprintSpeed = sm.manager.core.walkSpeed * sm.relativeSprintSpeed;
	}

	public override void OnUpdate() {
	}

}
