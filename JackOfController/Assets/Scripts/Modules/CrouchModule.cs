using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public class CrouchModule : Module {

    [Tooltip( "Toggle crouch or hold to crouch" )]
    public bool toggleCrouch = false;
    [Tooltip( "The height of the camera when crouching" )]
    public float crouchCamHeight = 0.5f;
    [Tooltip( "The height of the player (for collisions) while crouching" )]
    public float crouchPlayerHeight;
    [Tooltip( "How fast is the player when crouching?" )]
    public float crouchSpeed = 0f;
    [Tooltip( "How much slower or faster is the player while crouching, only used if crouchSpeed is 0" )]
    public float relativeCrouchSpeed = 0.5f;

    [HideInInspector] public bool crouching = false;
    [HideInInspector] public bool crouchCanceled = false;

	protected override void Awake() {
		base.Awake();
        state = CrouchedState.Instance;
        state.stateName = "CrouchedState";
	}

	public void OnCrouch( InputAction.CallbackContext value ) {
        if ( toggleCrouch ) {
            bool newCrouching = !crouching;

            if ( newCrouching ) {
                JoCManager.stateMachine.ChangeState( JoCManager.statesByName[ "CrouchedState" ] );
            }
			else {
                if ( CheckCeiling() ) {
                    crouchCanceled = true;
				}
				else {
                    JoCManager.stateMachine.ChangeState( JoCManager.statesByName[ "GroundedState" ] );
                }
			}
        }
        else {
            if ( value.performed ) {
                JoCManager.stateMachine.ChangeState( JoCManager.statesByName[ "CrouchedState" ] );
            }
            if ( value.canceled ) {
                if ( CheckCeiling() ) {
                    crouchCanceled = true;
                }
                else {
                    JoCManager.stateMachine.ChangeState( JoCManager.statesByName[ "GroundedState" ] );
                }
            }
        }
    }

    public bool CheckCeiling() {
        float radius = JoCManager.joc.playerStartHeight / 4f;
        return Physics.CheckSphere( new Vector3( JoCManager.joc.cc.transform.position.x, 
            JoCManager.joc.cc.transform.position. y + ( radius * 2f ),
            JoCManager.joc.transform.position.z ), radius, JoCManager.joc.groundMask );
    }

}
