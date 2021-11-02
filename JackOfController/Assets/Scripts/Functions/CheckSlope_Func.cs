using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "CheckSlope", menuName = "Functions/CheckSlope" )]
public class CheckSlope_Func : Function {

    private Slide_Module sm;

    public override void Init() {
        sm = manager.modulesByName[ "SlideModule" ] as Slide_Module;
		if ( sm == null ) Debug.LogError( "You cannot use the CheckSlope function without the Slide Module." );
	}

	public override void ExecuteFunction() {
		sm.slopeCheckOrigin = new Vector3( sm.transform.position.x, sm.transform.position.y - ( sm.manager.cc.height / 2 ) + 
			sm.startDistanceFromBottom, sm.transform.position.z );
		RaycastHit hit;
		if ( Physics.SphereCast( sm.slopeCheckOrigin, sm.sphereCastRadius, Vector3.down, out hit, sm.sphereCastDistance, sm.castingMask ) ) {
			sm.groundSlopeAngle = Vector3.Angle( hit.normal, Vector3.up );
			Vector3 temp = Vector3.Cross( hit.normal, Vector3.down );
			sm.slopeDir = Vector3.Cross( temp, hit.normal );
		}

		RaycastHit slopeHit1;
		RaycastHit slopeHit2;

		if ( Physics.Raycast( sm.slopeCheckOrigin + sm.rayOriginOffset1, Vector3.down, out slopeHit1, sm.raycastLength ) ) {

			if ( sm.showDebug ) { Debug.DrawLine( sm.slopeCheckOrigin + sm.rayOriginOffset1, slopeHit1.point, Color.red ); }

			float angleOne = Vector3.Angle( slopeHit1.normal, Vector3.up );

			if ( Physics.Raycast( sm.slopeCheckOrigin + sm.rayOriginOffset2, Vector3.down, out slopeHit2, sm.raycastLength ) ) {

				if ( sm.showDebug ) { Debug.DrawLine( sm.slopeCheckOrigin + sm.rayOriginOffset2, slopeHit2.point, Color.red ); }

				float angleTwo = Vector3.Angle( slopeHit2.normal, Vector3.up );
				List<float> tempArray = new List<float>() { sm.groundSlopeAngle, angleOne, angleTwo };
				tempArray.Sort();
				sm.groundSlopeAngle = tempArray[ 1 ];
			}
			else {
				float average = ( sm.groundSlopeAngle + angleOne ) / 2;
				sm.groundSlopeAngle = average;
			}
		}
	}

}
