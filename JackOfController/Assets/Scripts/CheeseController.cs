using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AerialMovementSettings {
    FullMovement,
    FullCameraMovement,
    LimitedCameraMovement,
    NoMovement
}

public class CheeseController : MonoBehaviour {

    public CharacterController cc;
    public Camera cam;

    [Header( "Movement Settings" )]
    [Tooltip("Speed of movement in units per second")]
    public float speed = 5f;
    public float stickToGroundForce = 1f;
    public AnimationCurve startSprintCurve;
    public AnimationCurve endSprintCurve;

    [Header( "Jump Settings" )]
    [Tooltip( "The upward force applied when the player jumps" )]
    public float jumpForce = 5f;
    [Tooltip( "How long it takes to complete and uninterrupted jump" )]
    public float jumpDuration = 1f;
    [Tooltip( "How the jump force is applied" )]
    public AnimationCurve jumpArc;
    [Tooltip( "How many times the player can jump" )]
    public int jumps = 1;

    [Header( "Sprint Settings" )]
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

    [Header( "Aerial Movement Settings" )]
    [Tooltip( "How much freedom of movement should the player have in mid-air?" )]
    public AerialMovementSettings aerialMovement;
    [Tooltip( "How fast can the player change direction in mid-air with limited camera movement" )]
    public float aerialTurnSpeed;

    private PlayerInput playerInput;
    private Rigidbody rb;
    private bool sprinting = false;
    private bool jump = false;
    private bool grounded = true;
    private float jumpTimeStamp;
    private Vector2 rawMovementVector;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        playerInput.ActivateInput();
        playerInput.SwitchCurrentActionMap( "PlayerControls" );
	}

	public void OnMove( InputAction.CallbackContext value ) {
        rawMovementVector = value.ReadValue<Vector2>();
    }

    public void OnSprint( InputAction.CallbackContext value ) {
        if ( value.started ) sprinting = true;
        if ( value.canceled ) sprinting = false;
    }

    public void OnJump( InputAction.CallbackContext value ) {
        if ( value.performed ) {
            jump = true;
		}
    }

	private void Update() {
        float moveSpeed = speed;

        if ( sprintAllowed ) {
            if ( sprinting ) {
                if ( relativeSprintSpeed == 0f ) {
                    moveSpeed = sprintSpeed;
                }
                else {
                    moveSpeed = speed * relativeSprintSpeed;
                }
            }
        }

        if ( jump ) {
            jumpTimeStamp = Time.time;
            jump = false;
        }

        Vector3 forwardVector = ( transform.position + cam.transform.forward - transform.position ).normalized;
        Vector3 rightVector = ( transform.position + cam.transform.right - transform.position ).normalized;
        Vector3 relativeMovementVector = rawMovementVector.x * rightVector + rawMovementVector.y * forwardVector;
        Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * moveSpeed, -stickToGroundForce, relativeMovementVector.z * moveSpeed );

        if ( jumpTimeStamp != 0f ) {
            float timePassed = Time.time - jumpTimeStamp;
            if ( timePassed <= jumpDuration ) {
                float adjustedJumpForce = jumpForce * jumpArc.Evaluate( timePassed / jumpDuration );
                finalMovementVector += new Vector3( 0f, stickToGroundForce + adjustedJumpForce, 0f );
            }
            else {
                jumpTimeStamp = 0f;
            }
        }

        cc.Move( finalMovementVector * Time.deltaTime );
	}
}
