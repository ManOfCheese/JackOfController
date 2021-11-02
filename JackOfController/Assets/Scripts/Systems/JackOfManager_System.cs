using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

[CreateAssetMenu( fileName = "JackOfManager_System", menuName = "Systems/JackOfManager_System" )]
public class JackOfManager_System : ComponentSystem {

	public JackOfManager manager;

	public override void Init() {
		//Find references to crucial components.
		manager.core = manager.GetComponent<Core_Module>();
		if ( manager.core == null ) Debug.LogError( "Core Module not found." );

		manager.cam = manager.transform.parent.GetComponentInChildren<Camera>();
		if ( manager.cam == null ) Debug.LogError( "Camera not found." );

		manager.cc = manager.transform.parent.GetComponent<CharacterController>();
		if ( manager.cc == null ) Debug.LogError( "Character Controller not found." );

		manager.stateMachine = new FiniteStateMachine();

		//Set base values for important variables.
		manager.currentSpeed = manager.core.walkSpeed;
		manager.playerStartHeight = manager.cc.height;
		manager.camStartHeight = manager.cam.transform.localPosition.y;
		manager.currentCamHeight = manager.camStartHeight;
		Cursor.lockState = CursorLockMode.Locked;

		//Find all modules and put them in a dictionary.
		Module[] modules = manager.GetComponents<Module>();

		manager.modulesByName = new Dictionary<string, Module>();
		for ( int i = 0; i < modules.Length; i++ ) {
			manager.modulesByName.Add( modules[ i ].moduleName, modules[ i ] );
			modules[ i ].manager = manager;
		}

		//Find all states, put them in a dictionary and initialize them.
		List<State> states = new List<State>();
		for ( int i = 0; i < modules.Length; i++ ) {
			for (int j = 0; j < modules[ i ].states.Length; j++) {
				states.Add( modules[ i ].states[ j ] );
			}
		}

		manager.statesByName = new Dictionary<string, State>();
		for ( int i = 0; i < states.Count; i++ ) {
			manager.statesByName.Add( states[ i ].stateName, states[ i ] );
			states[ i ].manager = this.manager;
			states[ i ].Init();
		}

		//Give all functions a reference to the manager.
		for ( int i = 0; i < manager.functions.Length; i++ ) {
			manager.functions[ i ].manager = this.manager;
			manager.functions[ i ].Init();
		}

		//Enter the default state.
		manager.stateMachine.ChangeState( manager.statesByName[ "GroundedState" ] );
	}

	public override void OnUpdate() {
		manager.stateMachine.Update();
	}

}
