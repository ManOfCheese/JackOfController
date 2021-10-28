using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "JackOfManager_System", menuName = "Systems/JackOfManagerSystem" )]
public class JackOfManagerSystem : ComponentSystem {

	public JackOfManager jom;

	public override void Init() {
		jom.currentSpeed = jom.joc.walkSpeed;
		jom.playerStartHeight = jom.cc.height;
		jom.camStartHeight = jom.cam.transform.localPosition.y;
		jom.currentCamHeight = jom.camStartHeight;
		Cursor.lockState = CursorLockMode.Locked;

		Module[] modules = jom.GetComponents<Module>();

		jom.modulesByName = new Dictionary<string, Module>();
		for ( int i = 0; i < modules.Length; i++ ) {
			jom.modulesByName.Add( modules[ i ].moduleName, modules[ i ] );
			modules[ i ].jom = jom;
		}

		List<State> states = new List<State>();
		for ( int i = 0; i < modules.Length; i++ ) {
			for (int j = 0; j < modules[ i ].states.Length; j++) {
				states.Add(modules[i].states[j]);
			}
		}

		jom.statesByName = new Dictionary<string, State>();
		for ( int i = 0; i < states.Count; i++ ) {
			jom.statesByName.Add( states[ i ].stateName, states[ i ] );
		}

		jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
	}

	public override void OnUpdate() {
		jom.stateMachine.Update();
	}

}
