using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "Airborne_State", menuName = "States/Airborne_State" )]
public class Airborne_State : State {

	public override void Init() {

	}

	public override void EnterState() {

	}

	public override void UpdateState() {
		for ( int i = 0; i < functionsToUpdate.Length; i++ ) {
			functionsToUpdate[ i ].ExecuteFunction();
		}
	}

	public override void ExitState() {

	}
}
