using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Jump", menuName = "Functions/Jump" )]
public class Jump_Func : Function {

    private Core_Module core;
    private Jump_Module jm;

    public override void Init() {
        jm = manager.modulesByName[ "JumpModule" ] as Jump_Module;
        if ( jm == null ) Debug.LogError( "You cannot use the Jump function without the Jump Module." );
        core = manager.core;
    }

    public override void ExecuteFunction() {
        if ( jm.jump && ( core.grounded || jm.jumpCount < jm.jumps ) ) {
            core.velocity.y = Mathf.Sqrt( jm.jumpHeight * -2 * core.gravity );
            jm.jump = false;
            if ( jm.aerialMovement != AerialMovementSettings.FullMovement ) jm.velocityOnJump = jm.manager.cc.velocity;
            jm.jumpCount++;
        }
        jm.manager.cc.Move( ( jm.velocityOnJump + core.velocity ) * Time.deltaTime );
    }

}
