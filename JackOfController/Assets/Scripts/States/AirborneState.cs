using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "AirborneState", menuName = "States/AirborneState" )]
public class AirborneState : State {

	public override void EnterState() {

	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}

		//_owner.joc.CameraLook();
		//_owner.joc.CheckSprint();
		//_owner.joc.Walk();
		//_owner.joc.LookMove();
		//_owner.joc.Gravity();
		//_owner.joc.Jump();
		//_owner.joc.CheckGround();
	}

	public override void ExitState() {

	}
}
