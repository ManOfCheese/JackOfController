using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public class Module : MonoBehaviour {

    [HideInInspector] public JackOfManager jom;
    public string moduleName;

    //State
    [HideInInspector] public State[] states;

    protected PlayerInput playerInput;

    protected virtual void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap( "PlayerControls" );
    }

}
