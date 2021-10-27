﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSprint : Function {

    private SprintModule spm;

    public void Init( SprintModule _SpM ) {
        this.spm = _SpM;
    }

    public override void ExecuteFunction() {
        if ( spm.sprintMode == SprintMode.HoldToSprint ) {
            if ( spm.sprintStart ) {
                spm.sprintStart = false;
                StartSprint();
            }
            else if ( sprintEnd ) {
                spm.sprintEnd = false;
                EndSprint();
            }
        }
        else if ( spm.sprintMode == SprintMode.SinglePressSprint ) {
            if ( spm.sprintStart ) {
                spm.sprintStart = false;
                spm.StartSprint();
            }
            else if ( !moving ) {
                EndSprint();
            }
        }
        else if ( spm.sprintMode == SprintMode.ToggleSprint ) {
            if ( spm.sprintToggle ) {
                bool newSprinting = !spm.sprinting;
                spm.sprintToggle = false;

                if ( newSprinting ) {
                    StartSprint();
                }
                else {
                    EndSprint();
                }
            }
        }
    }

    private void StartSprint() {
        spm.sprinting = true;
        if ( spm.sprintSpeed != 0f )
            spm.jom.currentSpeed = spm.sprintSpeed;
        else
            spm.jom.currentSpeed = spm.joc.walkSpeed * spm.relativeSprintSpeed;
    }

    private void EndSprint() {
        spm.sprinting = false;
        spm.jom.currentSpeed = walkSpeed;
    }
}
