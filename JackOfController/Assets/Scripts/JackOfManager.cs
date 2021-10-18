using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class JackOfManager : MonoBehaviour {

	public JackOfController joc;

	//Non-serializable
	public StateMachine<JackOfManager> stateMachine;
	public Dictionary<string, Module> modulesByName;
	public Dictionary<string, State<JackOfManager>> statesByName;

	private void Start() {
		stateMachine = new StateMachine<JackOfManager>( this );
		joc.jocManager = this;

		Module[] modules = GetComponents<Module>();

		modulesByName = new Dictionary<string, Module>();
		for ( int i = 0; i < modules.Length; i++ ) {
			modulesByName.Add( modules[ i ].moduleName, modules[ i ] );
			modules[ i ].jocManager = this;
		}

		List<State<JackOfManager>> states = new List<State<JackOfManager>>();
		states.Add( joc.groundedState );
		states.Add( joc.airborneState );
		for ( int i = 0; i < modules.Length; i++ ) {
			if ( modules[ i ].state != null ) states.Add( modules[ i ].state );
		}

		statesByName = new Dictionary<string, State<JackOfManager>>();
		for ( int i = 0; i < states.Count; i++ ) {
			statesByName.Add( states[ i ].stateName, states[ i ] );
		}

		stateMachine.ChangeState( statesByName[ "GroundedState" ] );
	}

	private void Update() {
		stateMachine.Update();
	}
}
