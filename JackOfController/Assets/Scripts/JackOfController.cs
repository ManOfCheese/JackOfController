using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JackOfController : Module {

    [Header( "Camera Settings" )]
    [Tooltip( "When true camera controls will be inverted meaning moving left will move the camera to the right." )]
    public bool inverted;
    [Tooltip( "Determines how much the camera moves relative to the input." )]
    [Range( 0.0f, 1.0f )]
    public float sensitivity;
    [Tooltip( "How many degrees the camera can rotate upwards before locking in place." )]
    public float xRotationLimitsUp = -90f;
    [Tooltip( "How many degrees the camera can rotate downwards before locking in place" )]
    public float xRotationLimitsDown = 60f;

    [Header( "Movement Settings" )]
    [Tooltip("Speed of movement in units per second")]
    public float walkSpeed = 5f;
    [Tooltip( "How fast does the player fall" )]
    public float gravity = -9.81f;
    public float stickToGroundForce = 10f;

    [Header( "Groundcheck Settings" )]
    public float groundDistance;
    public LayerMask groundMask;

    //Dependencies
    [HideInInspector] public CharacterController cc;

    //Optional
    [HideInInspector] public SprintModule spm;

    //Runtime
    [ReadOnly] public bool moving = false;
    [ReadOnly] public bool grounded = true;
    [ReadOnly] public float xCamRotation = 0.0f;
    [ReadOnly] public float yCamRotation = 0.0f;
    [ReadOnly] public Vector2 lookVector;
    [ReadOnly] public Vector2 rawMovementVector;
    [ReadOnly] public Vector3 velocity;

    #region Input
	public void OnLook( InputAction.CallbackContext value ) {
        Vector2 mouseLook = value.ReadValue<Vector2>();
        lookVector = new Vector2( mouseLook.y, mouseLook.x );
    }

    public void OnMove( InputAction.CallbackContext value ) {
        rawMovementVector = value.ReadValue<Vector2>();
        if ( value.started )
            moving = true;
        if ( value.canceled )
            moving = false;
    }
	#endregion

	//public void OnDrawGizmos() {
 //       float radius = jom.playerStartHeight / 4;

 //       Gizmos.color = Color.yellow;
 //       Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, cc.transform.position.y - radius, 
 //           cc.transform.position.z ), groundDistance );

 //       Gizmos.color = Color.red;
 //       Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, cc.transform.position.y + radius, 
 //           cc.transform.position.z ), groundDistance );
 //   }
}
