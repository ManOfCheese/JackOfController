using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "HeadBob_System", menuName = "Systems/HeadBob_System" )]
public class HeadBob_System : ComponentSystem {

    public HeadBob_Module hbm;
	private Sprint_Module sm;

	public override void Init() {
		hbm.currentHeadBobSpeed = hbm.headBobSpeed;
		if ( hbm.manager.modulesByName[ "SprintModule" ] )
			sm = hbm.manager.modulesByName[ "SprintModule" ] as Sprint_Module;
	}

	public override void OnUpdate() {
		if ( hbm.adjustSpeedWhenSprinting && sm ) {
			if ( sm.sprinting ) {
				float speedDifferential;
				speedDifferential = sm.sprintSpeed / hbm.manager.core.walkSpeed;

				hbm.currentHeadBobSpeed = hbm.headBobSpeed * speedDifferential;
			}
			else {
				hbm.currentHeadBobSpeed = hbm.headBobSpeed;
			}
		}

		if ( hbm.manager.core.rawMovementVector != Vector2.zero && hbm.manager.core.grounded ) {
			//Use a sine functions to move the camera up and down.
			hbm.manager.cam.transform.localPosition = new Vector3( 0.0f,
				hbm.manager.currentCamHeight + ( Mathf.Sin( Time.fixedTime * Mathf.PI * hbm.currentHeadBobSpeed ) * hbm.headBobIntensity ), 0.0f );
		}
	}

}
