using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class JackOfControllerManager : MonoBehaviour {

	public JackOfController bc;
	public Dictionary<string, Module> modules;
	private StateMachine<JackOfControllerManager> stateMachine;
	private List<State<JackOfControllerManager>> states;

}
