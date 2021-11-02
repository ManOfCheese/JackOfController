using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "StickToGround", menuName = "Functions/StickToGround" )]
public class StickToGround_Func : Function {

    private Core_Module core;

    public override void Init() {
        core = manager.core;
    }

    public override void ExecuteFunction() {
        core.velocity.y = core.stickToGroundForce * -1;
        core.manager.cc.Move( core.velocity );
    }

}
