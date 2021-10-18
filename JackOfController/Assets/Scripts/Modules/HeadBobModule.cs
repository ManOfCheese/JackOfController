using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobModule : Module {

    [Tooltip( "How quickly the camera moves up and down." )]
    public float headBobSpeed;
    [Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
    public float headBobIntensity;

	private void Update() {
		if( jocManager.joc.rawMovementVector != Vector2.zero ) {
			//Use a sine functions to move the camera up and down.
			jocManager.joc.cam.transform.localPosition = new Vector3( 0.0f,
				jocManager.joc.currentCamHeight + ( Mathf.Sin( Time.fixedTime * Mathf.PI * headBobSpeed ) * headBobIntensity ), 0.0f );
		}
	}
}
