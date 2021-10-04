using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour {

	[Header( "Camera Settings" )]
	[Tooltip("When true camera controls will be inverted meaning moving left will move the camera to the right.")]
	public bool inverted;
	[Tooltip( "Determines how much the camera moves relative to the input." )]
	[Range( 0.0f, 1.0f)]
	public float senstivity;
	[Tooltip( "How many degrees the camera can rotate upwards before locking in place." )]
	public float xRotationLimitsUp = -90f;
	[Tooltip( "How many degrees the camera can rotate downwards before locking in place" )]
	public float xRotationLimitsDown = 60f;

	[Header( "Crouch Settings" )]
	[Tooltip( "The height of the camera when crouching" )]
	public float crouchCamHeight = 0.5f;

	[Header( "Head Bob Settings" )]
	[Tooltip( "When true the camera moves up and down to simulate the movement of someone walking." )]
	public bool headBob;
	[Tooltip( "How quickly the camera moves up and down." )]
	public float headBobSpeed;
	[Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
	public float headBobIntensity;

	[Header( "FOV Boost Settings" )]
	[Tooltip( "How long does the FOV boost last." )]
	public float FOVBoostDuration;
	[Tooltip( "How much does the FOV increase." )]
	public float FOVBoostIntensity;
	[Tooltip( "Curve used to increase and decrease FOV when FOV boost happens." )]
	public AnimationCurve FOVBoostCurve;

	[Header( "Screen Tilt Settings" )]
	[Tooltip( "How many degrees does the screen tilt." )]
	public float maximumTilt;

	private Camera cam;
	private float camHeight;
	private float camStartHeight;
	private float xCamRotation = 0.0f;
	private float yCamRotation = 0.0f;
	public Vector2 lookVector;

	private void Awake() {
		cam = GetComponent<Camera>();
		camStartHeight = cam.transform.localPosition.y;
		camHeight = camStartHeight;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void OnLook( InputAction.CallbackContext value ) {
		Vector2 mouseLook = value.ReadValue<Vector2>();
		lookVector = new Vector2( mouseLook.y, mouseLook.x );
	}

	public void OnCrouch( InputAction.CallbackContext value ) {
		Debug.Log( "crouch" );
		camHeight = crouchCamHeight;
	}

	private void Update() {
		xCamRotation -= senstivity * lookVector.x;
		yCamRotation += senstivity * lookVector.y;

		xCamRotation %= 360;
		yCamRotation %= 360;
		xCamRotation = Mathf.Clamp( xCamRotation, xRotationLimitsUp, xRotationLimitsDown );
		transform.eulerAngles = new Vector3( xCamRotation, yCamRotation, 0f );
	}
}
