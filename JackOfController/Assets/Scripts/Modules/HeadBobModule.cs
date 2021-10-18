using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobModule : Module {

	[Tooltip( "HWhen true the head bob speed will change when sprinting." )]
	public bool adjustSpeedWhenSprinting;
    [Tooltip( "How quickly the camera moves up and down." )]
    public float headBobSpeed;
    [Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
    public float headBobIntensity;

	private float currentHeadBobSpeed;

	private void Awake() {
		currentHeadBobSpeed = headBobSpeed;
	}

	private void Update() {
		if ( adjustSpeedWhenSprinting ) {
			if ( jocManager.joc.sprinting ) {
				float speedDifferential;
				if ( jocManager.joc.relativeSprintSpeed != 0 )
					speedDifferential = jocManager.joc.speed * jocManager.joc.relativeSprintSpeed / jocManager.joc.speed;
				else
					speedDifferential = jocManager.joc.sprintSpeed / jocManager.joc.speed;

				currentHeadBobSpeed = headBobSpeed * speedDifferential;
			}
			else {
				currentHeadBobSpeed = headBobSpeed;
			}
		}

		if( jocManager.joc.rawMovementVector != Vector2.zero && jocManager.joc.grounded ) {
			//Use a sine functions to move the camera up and down.
			jocManager.joc.cam.transform.localPosition = new Vector3( 0.0f,
				jocManager.joc.currentCamHeight + ( Mathf.Sin( Time.fixedTime * Mathf.PI * headBobSpeed ) * headBobIntensity ), 0.0f );
		}
	}
}
