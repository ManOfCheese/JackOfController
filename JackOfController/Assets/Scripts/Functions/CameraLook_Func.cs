using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "CameraLook", menuName = "Functions/CameraLook" )]
public class CameraLook_Func : Function {

    private Core_Module core;

    public override void Init() {
        core = manager.core;
    }

    public override void ExecuteFunction() {
        core.xCamRotation -= core.sensitivity * core.lookVector.x;
        core.yCamRotation += core.sensitivity * core.lookVector.y;

        core.xCamRotation %= 360;
        core.yCamRotation %= 360;
        core.xCamRotation = Mathf.Clamp( core.xCamRotation, core.xRotationLimitsUp, core.xRotationLimitsDown );
        core.manager.cam.transform.eulerAngles = new Vector3( core.xCamRotation, core.yCamRotation, core.manager.cam.transform.eulerAngles.z );
        core.manager.cc.transform.eulerAngles = new Vector3( core.manager.cc.transform.eulerAngles.x, core.yCamRotation,
            core.manager.cc.transform.eulerAngles.z );
    }

}
