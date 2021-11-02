using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "Sprint_State", menuName = "States/Sprint_State" )]
public class Sprint_State : State {

	private Sprint_Module sm;

	public override void Init() {
		sm = manager.modulesByName[ "SprintModule" ] as Sprint_Module;
	}

	public override void EnterState() {
		sm.sprinting = true;
		sm.manager.currentSpeed = sm.sprintSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}
	}

	public override void ExitState() {
		sm.sprinting = false;
		sm.manager.currentSpeed = sm.manager.core.walkSpeed;
	}

}
