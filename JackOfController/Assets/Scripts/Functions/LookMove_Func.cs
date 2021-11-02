using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "LookMove", menuName = "Functions/LookMove" )]
public class LookMove_Func : Function {

    private Jump_Module jm;

    public void Init() {
        jm = manager.modulesByName[ "JumpModule" ] as Jump_Module;
        if ( jm == null ) Debug.LogError( "You cannot use the LookMove function without the Jump Module." );
    }

    public override void ExecuteFunction() {
        if ( jm.aerialMovement == AerialMovementSettings.FullCameraMovement || 
            jm.aerialMovement == AerialMovementSettings.LimitedCameraMovement ) {
            Vector3 camVector = new Vector3( jm.manager.cam.transform.forward.x, 0f, jm.manager.cam.transform.forward.z );

            if ( jm.aerialMovement == AerialMovementSettings.FullCameraMovement ) {
                jm.velocityOnJump = camVector * jm.manager.currentSpeed;
            }
            if ( jm.aerialMovement == AerialMovementSettings.LimitedCameraMovement ) {
                jm.velocityOnJump = ( ( ( camVector * jm.aerialTurnSpeed ) + jm.velocityOnJump ) / 2f ).normalized * jm.manager.currentSpeed;
            }
        }
    }

}
