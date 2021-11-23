using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AerialMovementSettings {
    FullMovement,
    FullCameraMovement,
    LimitedCameraMovement,
    NoMovement
}

public class Jump_Module : Module {

    [Header( "Jump Settings" )]
    [Tooltip( "Height of the jump" )]
    public float jumpHeight = 5f;
    [Tooltip( "How many times the player can jump" )]
    public int jumps = 1;
    [Tooltip( "How fast can the player change direction in mid-air with limited camera movement" )]
    public float aerialTurnSpeed;

    [Header( "Debug" )]
    [ReadOnly] public bool jump = false;
    [ReadOnly] public int jumpCount;
    [ReadOnly] public Vector3 velocityOnJump;

	protected override void Awake() {
        if ( moduleName == "" ) moduleName = "JumpModule";
    }

	public void OnJump( InputAction.CallbackContext value ) {
        if ( value.performed && ( manager.core.grounded || jumpCount < jumps ) ) jump = true;
    }

}
