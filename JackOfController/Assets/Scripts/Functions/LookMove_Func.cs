using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "LookMove", menuName = "Functions/LookMove" )]
public class LookMove_Func : Function {

    private Jump_Module jm;

    public override void Init() {
        jm = manager.modulesByName[ "JumpModule" ] as Jump_Module;
        if ( jm == null ) Debug.LogError( "You cannot use the LookMove function without the Jump Module." );
    }

    public override void ExecuteFunction() {
        Vector3 camVector = new Vector3( jm.manager.cam.transform.forward.x, 0f, jm.manager.cam.transform.forward.z );

        jm.velocityOnJump = ( ( ( camVector * jm.aerialTurnSpeed ) + jm.velocityOnJump ) ).normalized * jm.manager.currentSpeed;
    }

}
