using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StateMachine;

public enum AerialMovementSettings {
    FullMovement,
    FullCameraMovement,
    LimitedCameraMovement,
    NoMovement
}

public class JackOfController : MonoBehaviour {

    [Header( "References" )]
    public StateMachine<JackOfController> stateMachine;
    public CharacterController cc;
    public Camera cam;

    //Camera Settings
    [Tooltip( "When true camera controls will be inverted meaning moving left will move the camera to the right." )]
    public bool inverted;
    [Tooltip( "Determines how much the camera moves relative to the input." )]
    [Range( 0.0f, 1.0f )]
    public float sensitivity;
    [Tooltip( "How many degrees the camera can rotate upwards before locking in place." )]
    public float xRotationLimitsUp = -90f;
    [Tooltip( "How many degrees the camera can rotate downwards before locking in place" )]
    public float xRotationLimitsDown = 60f;

    //Head Bob Settings
    [Tooltip( "When true the camera moves up and down to simulate the movement of someone walking." )]
    public bool headBob;
    [Tooltip( "How quickly the camera moves up and down." )]
    public float headBobSpeed;
    [Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
    public float headBobIntensity;

    //FOV Boost Settings
    [Tooltip( "How long does the FOV boost last." )]
    public float FOVBoostDuration;
    [Tooltip( "How much does the FOV increase." )]
    public float FOVBoostIntensity;
    [Tooltip( "Curve used to increase and decrease FOV when FOV boost happens." )]
    public AnimationCurve FOVBoostCurve;

    //Screen Tilt Settings
    [Tooltip( "How many degrees does the screen tilt." )]
    public float maximumTilt;

    //Movement Settings
    [Tooltip("Speed of movement in units per second")]
    public float speed = 5f;
    [Tooltip( "How fast does the player fall" )]
    public float gravity;
    public float stickToGroundForce = 1f;

    //Groundcheck Settings
    public float groundDistance;
    public LayerMask groundMask;

    //Jump Settings
    [Tooltip( "Height of the jump" )]
    public float jumpHeight = 5f;
    [Tooltip( "How many times the player can jump" )]
    public int jumps = 1;


    //Sprint Settings
    [Tooltip( "Is sprinting enabled" )]
    public bool sprintAllowed = true;
    [Tooltip( "Should the FOV momentarily increase when you start sprinting" )]
    public bool FOVBurst = true;
    [Tooltip( "Should the FOV increase when you start sprinting" )]
    public bool FOVBoost = true;
    [Tooltip( "Sprinting speed in units per second" )]
    public float sprintSpeed = 10f;
    [Tooltip( "Sprinting speed relative to the walking speed" )]
    public float relativeSprintSpeed = 2f;
    public AnimationCurve startSprintCurve;
    public AnimationCurve endSprintCurve;

    //Aerial Movement Settings
    [Tooltip( "How much freedom of movement should the player have in mid-air?" )]
    public AerialMovementSettings aerialMovement;
    [Tooltip( "How fast can the player change direction in mid-air with limited camera movement" )]
    public float aerialTurnSpeed;

    private PlayerInput playerInput;

    //Camera
    public float playerStartHeight;
    private float camStartHeight;
    private float xCamRotation = 0.0f;
    private float yCamRotation = 0.0f;
    public Vector2 lookVector;

    //Movement
    private bool sprinting = false;
    private bool jump = false;
    private bool grounded = true;
    public float currentSpeed;
    private Vector2 rawMovementVector;
    public Vector3 velocity;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap( "PlayerControls" );

        playerStartHeight = cc.height;
        camStartHeight = cam.transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
    }

    #region Input
	public void OnLook( InputAction.CallbackContext value ) {
        Vector2 mouseLook = value.ReadValue<Vector2>();
        lookVector = new Vector2( mouseLook.y, mouseLook.x );
    }

    public void OnMove( InputAction.CallbackContext value ) {
        rawMovementVector = value.ReadValue<Vector2>();
    }

    public void OnSprint( InputAction.CallbackContext value ) {
        if ( value.started ) sprinting = true;
        if ( value.canceled ) sprinting = false;
    }

    public void OnJump( InputAction.CallbackContext value ) {
        if ( value.performed && grounded ) jump = true;
    }
	#endregion

	#region Movement
    public void CameraLook() {
        xCamRotation -= sensitivity * lookVector.x;
        yCamRotation += sensitivity * lookVector.y;

        xCamRotation %= 360;
        yCamRotation %= 360;
        xCamRotation = Mathf.Clamp( xCamRotation, xRotationLimitsUp, xRotationLimitsDown );
        cam.transform.eulerAngles = new Vector3( xCamRotation, yCamRotation, 0f );
        cc.transform.eulerAngles = new Vector3( cc.transform.eulerAngles.x, yCamRotation, cc.transform.eulerAngles.z );
    }

    public void Walk() {
        Vector3 relativeMovementVector = rawMovementVector.x * cc.transform.right + rawMovementVector.y * cc.transform.forward;
        Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * currentSpeed, velocity.y, relativeMovementVector.z * currentSpeed );
        cc.Move( finalMovementVector * Time.deltaTime );
    }

    public void Jump() {
        if ( jump ) {
            velocity.y = Mathf.Sqrt( jumpHeight * -2 * gravity );
            jump = false;
        }
        cc.Move( velocity * Time.deltaTime );
    }

    public void Gravity() {
        if ( !grounded )
            velocity.y += gravity * Time.deltaTime;
        else
            velocity.y = stickToGroundForce;
    }
	#endregion

	#region Checks
	public void CheckGround() {
        float radius = cc.height / 4f;
        grounded = Physics.CheckSphere( new Vector3( cc.transform.position.x, radius, cc.transform.position.z ), groundDistance, groundMask );
    }

    public void CheckSprint() {
        if ( sprintAllowed && !crouching ) {
            if ( sprinting ) {
                if ( sprintSpeed != 0f )
                    currentSpeed = sprintSpeed;
                else
                    currentSpeed = speed * relativeSprintSpeed;
            }
        }
    }
    #endregion

	public void OnDrawGizmos() {
        float radius = playerStartHeight / 4;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, radius, cc.transform.position.z ), groundDistance );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere( new Vector3( cc.transform.position.x, radius * 3f, cc.transform.position.z ), radius );
    }
}
