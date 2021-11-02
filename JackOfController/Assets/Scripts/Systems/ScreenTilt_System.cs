using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "ScreenTilt_System", menuName = "Systems/ScreenTilt_System" )]
public class ScreenTilt_System : ComponentSystem {

    public ScreenTilt_Module stm;

	public override void OnUpdate() {
		float tilt = stm.currentTilt;

		if ( stm.rawMovementVector.x != 0 ) {
			float zRotation = CustomMath.CustomMath.Map( stm.rawMovementVector.x, -1, 1, -stm.tiltSpeed, stm.tiltSpeed );
			tilt += zRotation * Time.deltaTime;
			tilt = Mathf.Clamp( tilt, -stm.maximumTilt, stm.maximumTilt );

			Camera cam = stm.manager.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}
		else {
			if ( stm.currentTilt != 0 ) {
				float adjustedTilt = stm.tiltSpeed * Time.deltaTime;
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
			Camera cam = stm.manager.cam;
			cam.transform.eulerAngles = new Vector3( cam.transform.rotation.x, cam.transform.rotation.y, -tilt );
		}

		stm.currentTilt = tilt;
	}

}
