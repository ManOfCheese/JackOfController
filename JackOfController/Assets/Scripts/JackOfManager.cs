using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class JackOfManager : MonoBehaviour {

	public JackOfManager_System system;
	[Space( 10 )]
	public Function[] functions;
	public ComponentSystem[] systemInitOrder;
	public ComponentSystem[] systemUpdateOrder;

	//Startup
	[ReadOnly] public float playerStartHeight;
	[ReadOnly] public float camStartHeight;

	//Runtime
	[ReadOnly] public float currentSpeed;
	[ReadOnly] public float currentCamHeight;
	[ReadOnly] public float currentPlayerHeight;
	[ReadOnly] public string currentState;

	[HideInInspector] public Core_Module core;
	[HideInInspector] public Camera cam;
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public FiniteStateMachine stateMachine;
	[HideInInspector] public Dictionary<string, Module> modulesByName;
	[HideInInspector] public Dictionary<string, State> statesByName;

	private void Awake() {
		system.manager = this;
	}

	private void Start() {
		for ( int i = 0; i < systemInitOrder.Length; i++ ) {
			systemInitOrder[ i ].Init();
		}
	}

	private void Update() {
		for ( int i = 0; i < systemUpdateOrder.Length; i++ ) {
			systemUpdateOrder[ i ].OnUpdate();
		}
		currentState = stateMachine.CurrentState.stateName;
	}

}
