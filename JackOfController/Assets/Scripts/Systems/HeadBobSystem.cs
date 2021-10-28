using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "HeadBob_System", menuName = "Systems/HeadBobSystem" )]
public class HeadBobSystem : ComponentSystem {

    public HeadBobModule hbm;

	public override void Init() {
		hbm.currentHeadBobSpeed = hbm.headBobSpeed;
	}

	public override void OnUpdate() {
		if ( hbm.adjustSpeedWhenSprinting ) {
			if ( hbm.spm.sprinting ) {
				float speedDifferential;
				speedDifferential = hbm.spm.rSprintSpeed / hbm.jom.joc.walkSpeed;

				hbm.currentHeadBobSpeed = hbm.headBobSpeed * speedDifferential;
			}
			else {
				hbm.currentHeadBobSpeed = hbm.headBobSpeed;
			}
		}

		if ( hbm.jom.joc.rawMovementVector != Vector2.zero && hbm.jom.joc.grounded ) {
			//Use a sine functions to move the camera up and down.
			hbm.jom.cam.transform.localPosition = new Vector3( 0.0f,
				hbm.jom.currentCamHeight + ( Mathf.Sin( Time.fixedTime * Mathf.PI * hbm.headBobSpeed ) * hbm.headBobIntensity ), 0.0f );
		}
	}

}
