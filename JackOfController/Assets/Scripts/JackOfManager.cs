using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class JackOfManager : MonoBehaviour {

	public JackOfManagerSystem system;
	[Space( 10 )]
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

	[HideInInspector] public JackOfController joc;
	[HideInInspector] public Camera cam;
	[HideInInspector] public CharacterController cc;
	[HideInInspector] public FiniteStateMachine stateMachine;
	[HideInInspector] public Dictionary<string, Module> modulesByName;
	[HideInInspector] public Dictionary<string, State> statesByName;

	private void Awake() {
		system.jom = this;
		joc.jom = this;
		cam = transform.parent.GetComponentInChildren<Camera>();
		cc = transform.parent.GetComponent<CharacterController>();
		stateMachine = new FiniteStateMachine();
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
