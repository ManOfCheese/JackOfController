using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob_Module : Module {

	public HeadBob_System system;

	[Header( "Head Bob Settings" )]
	[Tooltip( "HWhen true the head bob speed will change when sprinting." )]
	public bool adjustSpeedWhenSprinting = false;
    [Tooltip( "How quickly the camera moves up and down." )]
    public float headBobSpeed = 3f;
    [Tooltip( "How far up and down the camera moves, if  this is higher it the camera will also need to move faster to cover the distance." )]
    public float headBobIntensity = 0.2f;

	[Header( "Debug" )]
	[ReadOnly] public float currentHeadBobSpeed;

	//Optional
	[HideInInspector] public Sprint_Module spm;

	protected override void Awake() {
		system.hbm = this;
		if ( moduleName == "" ) moduleName = "HeadBobModule";
	}

}
