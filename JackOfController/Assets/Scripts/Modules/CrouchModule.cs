using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public class CrouchModule : Module {

    public bool sprintCrouch;
    [Tooltip( "Toggle crouch or hold to crouch" )]
    public bool toggleCrouch = false;
    [Tooltip( "The height of the camera when crouching" )]
    public float crouchCamHeight = 0.5f;
    [Tooltip( "The height of the player (for collisions) while crouching" )]
    public float crouchPlayerHeight = 0.8f;
    [Tooltip( "How fast is the player when crouching?" )]
    public float crouchSpeed = 0f;
    [Tooltip( "How much slower or faster is the player while crouching, only used if crouchSpeed is 0" )]
    public float relativeCrouchSpeed = 0.5f;

    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool crouchCanceled = false;

    //Dependencies
    [HideInInspector] public JackOfController joc;

    //Optional
    [HideInInspector] public SprintModule spm;

    protected override void Awake() {
		base.Awake();
        if ( jom.modulesByName["SprintModule"] != null) {
            spm = jom.modulesByName["SprintModule"] as SprintModule;
        }
	}

	public void OnCrouch( InputAction.CallbackContext value ) {
        State currentState = jom.stateMachine.CurrentState;

        if ( currentState != jom.statesByName[ "AirborneState" ] ) {
            if ( toggleCrouch ) {
                bool newCrouching = !crouching;

                if ( newCrouching && jom.stateMachine.CurrentState != jom.statesByName[ "CrouchedState" ] ) {
                    StartCrouch();
                }
                else {
                    EndCrouch();
                }
            }
            else {
                if ( value.performed && jom.stateMachine.CurrentState != jom.statesByName[ "CrouchedState" ] ) {
                    StartCrouch();
                }
                if ( value.canceled ) {
                    EndCrouch();
                }
            }
        }
    }

    private void StartCrouch() {
        if ( spm ) {
            if ( sprintCrouch || !spm.sprinting ) jom.stateMachine.ChangeState( jom.statesByName[ "CrouchedState" ] );
        }
		else {
            jom.stateMachine.ChangeState( jom.statesByName[ "CrouchedState" ] );
        }
    }

    private void EndCrouch() {
        if ( CheckCeiling() ) {
            crouchCanceled = true;
        }
        else if ( jom.stateMachine.CurrentState != jom.statesByName[ "GroundedState" ] ) {
            jom.stateMachine.ChangeState( jom.statesByName[ "GroundedState" ] );
        }
    }

    public bool CheckCeiling() {
        float radius = jom.playerStartHeight / 4f;
        return Physics.CheckSphere( new Vector3( jom.cc.transform.position.x, 
            jom.cc.transform.position. y + ( radius * 2f ),
            jom.joc.transform.position.z ), radius, jom.joc.groundMask );
    }

}
