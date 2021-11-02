using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Core_Module : Module {

    [Header( "Camera Settings" )]
    [Tooltip( "When true camera controls will be inverted meaning moving left will move the camera to the right." )]
    public bool inverted;
    [Tooltip( "Determines how much the camera moves relative to the input." )]
    [Range( 0.0f, 1.0f )]
    public float sensitivity = 0.3f;
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
    public float groundDistance = 0.6f;
    public LayerMask groundMask;

    [Header( "Debug" )]
    [ReadOnly] public bool moving = false;
    [ReadOnly] public bool grounded = true;
    [ReadOnly] public float xCamRotation = 0.0f;
    [ReadOnly] public float yCamRotation = 0.0f;
    [ReadOnly] public Vector2 lookVector;
    [ReadOnly] public Vector2 rawMovementVector;
    [ReadOnly] public Vector3 velocity;

	protected override void Awake() {
        if ( moduleName ==  "" ) moduleName = "CoreModule";
    }

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

    public bool CheckCeiling() {
        float radius = manager.playerStartHeight / 4f;
        return Physics.CheckSphere( new Vector3( manager.cc.transform.position.x,
            manager.cc.transform.position.y + ( radius * 2f ),
            manager.core.transform.position.z ), radius, manager.core.groundMask );
    }

}
