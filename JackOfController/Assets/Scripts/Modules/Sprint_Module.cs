using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SprintMode {
    HoldToSprint,
    ToggleSprint,
    SinglePressSprint
}

public class Sprint_Module : Module {

    public Sprint_System system;

    [Header( "Sprint Settings" )]
    [Tooltip( "Is sprinting enabled." )]
    public bool sprintAllowed = true;
    [Tooltip( "Sprinting speed in units per second." )]
    public float absSprintSpeed = 0f;
    [Tooltip( "Sprinting speed relative to the walking speed." )]
    public float relativeSprintSpeed = 2f;
    [Tooltip( "Determines how one initiates and ends a sprint." )]
    public SprintMode sprintMode;
    [Tooltip( "Determines the build of speed when you start sprinting." )]
    public AnimationCurve startSprintCurve;
    [Tooltip( "Determines the loss of speed when you stop sprinting." )]
    public AnimationCurve endSprintCurve;

    [Header( "Debug" )]
    [ReadOnly] public bool sprinting = false;

    [HideInInspector] public float sprintSpeed;
    [HideInInspector] public bool sprintStart = false;
    [HideInInspector] public bool sprintEnd = false;
    [HideInInspector] public bool sprintToggle = false;

	protected override void Awake() {
        system.sm = this;
        if ( moduleName == "" ) moduleName = "SprintModule";
    }

	public void OnSprint( InputAction.CallbackContext value ) {
        if ( value.started ) {
            sprintStart = true;
            sprintToggle = true;
        }
        if ( value.canceled ) {
            sprintEnd = true;
        }
    }

}
