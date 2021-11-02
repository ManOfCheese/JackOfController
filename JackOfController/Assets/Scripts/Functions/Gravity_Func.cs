using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Gravity", menuName = "Functions/Gravity" )]
public class Gravity_Func : Function {

    private Core_Module core;

    public override void Init() {
        core = manager.core;
    }

    public override void ExecuteFunction() {
        core.velocity.y += core.gravity * Time.deltaTime;
    }

}
