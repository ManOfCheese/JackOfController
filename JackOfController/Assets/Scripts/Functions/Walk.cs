using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : Function {

	private JackOfController joc;

	public void Init( JackOfController _joc ) {
		this.joc = _joc;
	}

	public override void ExecuteFunction() {
		Vector3 relativeMovementVector = joc.rawMovementVector.x * joc.jom.cc.transform.right + 
			joc.rawMovementVector.y * joc.jom.cc.transform.forward;
		Vector3 finalMovementVector = new Vector3( relativeMovementVector.x * joc.jom.currentSpeed, joc.velocity.y,
			relativeMovementVector.z * joc.jom.currentSpeed );
		joc.jom.cc.Move( finalMovementVector * Time.deltaTime );
	}
}
