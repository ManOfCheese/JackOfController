using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool crouching = false;
    public bool crouchCanceled = false;

	protected override void Awake() {
		base.Awake();
	}

	public void OnCrouch( InputAction.CallbackContext value ) {
        if ( toggleCrouch ) {
            bool newCrouching = !crouching;

            if ( newCrouching ) {
                //Enter crouch state.
            }
			else {
                if ( CheckCeiling() ) {
                    crouchCanceled = true;
				}
				else {
                    //Exit Crouch State.
				}
			}
        }
        else {
            if ( value.performed ) {
                //Enter crouch state.
            }
            if ( value.canceled ) {
                if ( CheckCeiling() ) {
                    crouchCanceled = true;
                }
                else {
                    //Exit Crouch State.
                }
            }
        }
    }

    public bool CheckCeiling() {
        float radius = JoCManager.bc.playerStartHeight / 4f;
        return Physics.CheckSphere( new Vector3( JoCManager.bc.cc.transform.position.x, radius * 3f,
            JoCManager.bc.transform.position.z ), radius, JoCManager.bc.groundMask );
    }

}
