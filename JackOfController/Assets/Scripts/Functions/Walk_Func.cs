using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "Walk", menuName = "Functions/Walk" )]
public class Walk_Func : Function {

	private Core_Module core;
	private CharacterController cc;

	public override void Init() {
		core = manager.core;
		cc = manager.cc;
	}

	public override void ExecuteFunction() {
		Vector3 relativeMovementVector = core.rawMovementVector.x * cc.transform.right + core.rawMovementVector.y * cc.transform.forward;
		Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * manager.currentSpeed, core.velocity.y,
			relativeMovementVector.z * manager.currentSpeed );
		cc.Move( finalMovementVector * Time.deltaTime );
	}
}
