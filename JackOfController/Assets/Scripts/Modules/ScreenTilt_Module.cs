using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenTilt_Module : Module {

	public ScreenTilt_System system;

	[Header( "Screen Tilt Settings" )]
	[Tooltip( "How many degrees does the screen tilt." )]
	public float maximumTilt = 2f;
	[Tooltip( "How fast does the tilting happen." )]
	public float tiltSpeed = 30f;

	[Header( "Debug" )]
	[ReadOnly] public float currentTilt;

	[HideInInspector] public Vector2 rawMovementVector;

	protected override void Awake() {
		system.stm = this;
		if ( moduleName == "" ) moduleName = "HeadBobModule";
	}

	public void OnMove( InputAction.CallbackContext value ) {
		rawMovementVector = value.ReadValue<Vector2>();
	}
}
