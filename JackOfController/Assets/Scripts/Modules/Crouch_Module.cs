using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public class Crouch_Module : Module {

    public Crouch_System system;

    [Header( "Crouch Settings" )]
    public bool sprintCrouch;
    [Tooltip( "Toggle crouch or hold to crouch" )]
    public bool toggleCrouch = false;
    [Tooltip( "The height of the camera when crouching" )]
    public float crouchCamHeight = 0.5f;
    [Tooltip( "The height of the player (for collisions) while crouching" )]
    public float crouchPlayerHeight = 0.8f;
    [Tooltip( "How fast is the player when crouching?" )]
    public float absCrouchSpeed = 0f;
    [Tooltip( "How much slower or faster is the player while crouching, only used if crouchSpeed is 0" )]
    public float relativeCrouchSpeed = 0.5f;

    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool crouchCanceled = false;
    [HideInInspector] public float crouchSpeed;
    [HideInInspector] public Sprint_Module sprintModule;
    [HideInInspector] public Crouch_State crouchState;
    [HideInInspector] public Airborne_State airborneState;
    [HideInInspector] public Grounded_State groundedState;

    protected override void Awake() {
        system.cm = this;
        if ( moduleName == "" ) moduleName = "CrouchModule";
    }

	public void OnCrouch( InputAction.CallbackContext value ) {
        State currentState = manager.stateMachine.CurrentState;

        if ( currentState != airborneState && currentState != manager.statesByName[ "SlideState" ] ) {
            if ( toggleCrouch ) {
                bool newCrouching = !crouching;

                if ( newCrouching && manager.stateMachine.CurrentState != crouchState ) {
                    StartCrouch();
                }
                else if ( manager.stateMachine.CurrentState == crouchState ) {
                    EndCrouch();
                }
            }
            else {
                if ( value.performed && manager.stateMachine.CurrentState != crouchState ) {
                    StartCrouch();
                }
                if ( value.canceled && manager.stateMachine.CurrentState == crouchState ) {
                    EndCrouch();
                }
            }
        }
    }

    private void StartCrouch() {
        if ( sprintModule ) {
            if ( sprintCrouch || !sprintModule.sprinting ) {
                manager.stateMachine.ChangeState( crouchState );
            }
        }
		else {
            manager.stateMachine.ChangeState( crouchState );
        }
    }

    private void EndCrouch() {
        if ( manager.core.CheckCeiling() ) {
            crouchCanceled = true;
        }
        if ( manager.core.grounded )
            manager.stateMachine.ChangeState( groundedState );
        else
            manager.stateMachine.ChangeState( manager.statesByName[ "AirborneState" ] );
    }

}
