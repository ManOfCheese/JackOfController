using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Module : MonoBehaviour {

    public JackOfControllerManager JoCManager;
    public string moduleName;

    private PlayerInput playerInput;

    protected virtual void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap( "PlayerControls" );
    }

}
