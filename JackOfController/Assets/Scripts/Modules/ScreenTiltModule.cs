using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScreenTiltModule : Module {

	[Tooltip( "How many degrees does the screen tilt." )]
	public float maximumTilt;
	[Tooltip( "How fast does the tilting happen." )]
	public float tiltSpeed;

	private float tilt;
	private Vector2 rawMovementVector;

	public void OnMove( InputAction.CallbackContext value ) {
		rawMovementVector = value.ReadValue<Vector2>();
	}

	private void Update() {
		if ( rawMovementVector.x != 0 ) {
			float zRotation = Map( rawMovementVector.x, -1, 1, -tiltSpeed, tiltSpeed );
			tilt += zRotation * Time.deltaTime;
			tilt = Mathf.Clamp( tilt, -maximumTilt, maximumTilt );

			Camera cam = jocManager.joc.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}
		else {
			if ( tilt != 0 ) {
				float adjustedTilt = tiltSpeed * Time.deltaTime;
				if ( tilt > 0 ) {
					if ( tilt - adjustedTilt < 0 ) 
						tilt = 0;
					else
						tilt -= adjustedTilt;
				}
				else {
					if ( tilt + adjustedTilt > 0 )
						tilt = 0;
					else
						tilt += adjustedTilt;
				}
			}
			Camera cam = jocManager.joc.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}
	}

	public static float Map( float value, float inputFrom, float inputTo, float outputFrom, float outputTo ) {
		return ( value - inputFrom ) / ( inputTo - inputFrom ) * ( outputTo - outputFrom ) + outputFrom;
	}
}
