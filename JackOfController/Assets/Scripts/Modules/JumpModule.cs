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

public class JumpModule : Module {

    [Header( "Jump Settings" )]
    [Tooltip( "Height of the jump" )]
    public float jumpHeight = 5f;
    [Tooltip( "How many times the player can jump" )]
    public int jumps = 1;
    [Tooltip( "How much freedom of movement should the player have in mid-air?" )]
    public AerialMovementSettings aerialMovement;
    [Tooltip( "How fast can the player change direction in mid-air with limited camera movement" )]
    public float aerialTurnSpeed;

    [ReadOnly] public bool jump = false;
    [ReadOnly] public int jumpCount;
    [ReadOnly] public Vector3 velocityOnJump;

    //Dependencies
    [HideInInspector] public JackOfController joc;

    public void OnJump( InputAction.CallbackContext value ) {
        if ( value.performed && ( base.jom.joc.grounded || jumpCount < jumps ) ) jump = true;
    }

}
