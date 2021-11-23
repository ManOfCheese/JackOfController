using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "Grounded_State", menuName = "States/Grounded_State" )]
public class Grounded_State : State {

	public override void Init() {

	}

	public override void EnterState() {
		manager.currentSpeed = manager.core.walkSpeed;
	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}
	}

	public override void ExitState() {

	}
}
