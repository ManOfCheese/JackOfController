using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : Function {

    private JackOfController joc;

    public void Init( JackOfController _joc ) {
        this.joc = _joc;
    }

    public override void ExecuteFunction() {
        joc.xCamRotation -= joc.sensitivity * joc.lookVector.x;
        joc.yCamRotation += joc.sensitivity * joc.lookVector.y;

        joc.xCamRotation %= 360;
        joc.yCamRotation %= 360;
        joc.xCamRotation = Mathf.Clamp( joc.xCamRotation, joc.xRotationLimitsUp, joc.xRotationLimitsDown );
        joc.jom.cam.transform.eulerAngles = new Vector3( joc.xCamRotation, joc.yCamRotation, joc.jom.cam.transform.eulerAngles.z );
        joc.jom.cc.transform.eulerAngles = new Vector3( joc.jom.cc.transform.eulerAngles.x, joc.yCamRotation,
            joc.jom.cc.transform.eulerAngles.z );
    }

}
