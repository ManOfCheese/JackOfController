using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookMove : Function {

    private JumpModule jm;

    public void Init( JumpModule _jm ) {
        jm = _jm;
    }

    public override void ExecuteFunction() {
        if ( jm.aerialMovement == AerialMovementSettings.FullCameraMovement || 
            jm.aerialMovement == AerialMovementSettings.LimitedCameraMovement ) {
            Vector3 camVector = new Vector3( jm.jom.cam.transform.forward.x, 0f, jm.jom.cam.transform.forward.z );

            if ( jm.aerialMovement == AerialMovementSettings.FullCameraMovement ) {
                jm.velocityOnJump = camVector * jm.jom.currentSpeed;
            }
            if ( jm.aerialMovement == AerialMovementSettings.LimitedCameraMovement ) {
                jm.velocityOnJump = ( ( ( camVector * jm.aerialTurnSpeed ) + jm.velocityOnJump ) / 2f ).normalized * jm.jom.currentSpeed;
            }
        }
    }

}
