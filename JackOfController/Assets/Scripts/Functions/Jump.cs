using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Function {

    private JumpModule jm;

    public void Init( JumpModule _jm ) {
        jm = _jm;
    }

    public override void ExecuteFunction() {
        if ( jm.jump && ( jm.joc.grounded || jm.jumpCount < jm.jumps ) ) {
            jm.joc.velocity.y = Mathf.Sqrt( jm.jumpHeight * -2 * jm.joc.gravity );
            jm.jump = false;
            if ( jm.aerialMovement != AerialMovementSettings.FullMovement ) jm.velocityOnJump = jm.jom.cc.velocity;
            jm.jumpCount++;
        }
        jm.jom.cc.Move( ( jm.velocityOnJump + jm.joc.velocity ) * Time.deltaTime );
    }

}
