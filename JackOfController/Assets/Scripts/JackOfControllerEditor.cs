using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JackOfController))]
public class JackOfControllerEditor : Editor {
    SerializedProperty cc;
    SerializedProperty cam;

    SerializedProperty inverted;
    SerializedProperty sensitivity;
    SerializedProperty xRotationLimitsUp;
    SerializedProperty xRotationLimitsDown;

    SerializedProperty headBob;
    SerializedProperty headBobSpeed;
    SerializedProperty headBobIntensity;

    SerializedProperty FOVBoostDuration;
    SerializedProperty FOVBoostIntensity;
    SerializedProperty FOVBoostCurve;

    SerializedProperty maximumTilt;

    SerializedProperty speed;
    SerializedProperty stickToGroundForce;
    SerializedProperty startSprintCurve;
    SerializedProperty endSprintCurve;

    SerializedProperty groundDistance;
    SerializedProperty groundMask;

    SerializedProperty jumpHeight;
    SerializedProperty jumps;
    SerializedProperty gravity;

    SerializedProperty sprintAllowed;
    SerializedProperty FOVBurst;
    SerializedProperty FOVBoost;
    SerializedProperty sprintSpeed;
    SerializedProperty relativeSprintSpeed;

    SerializedProperty toggleCrouch;
    SerializedProperty crouchCamHeight;
    SerializedProperty crouchPlayerHeight;
    SerializedProperty crouchSpeed;
    SerializedProperty relativeCrouchSpeed;

    SerializedProperty aerialMovement;
    SerializedProperty aerialTurnSpeed;

    bool showCameraSettings = false;
    bool showPlayerSettings = false;

    bool showHeadBobSettings = false;
    bool showFOVBoostSettings = false;
    bool ShowScreentiltSettings = false;

    bool showMovementSettings = false;
    bool showGroundCheckSettings = false;
    bool showJumpSettings = false;
    bool showSprintSettings = false;
    bool showCrouchSettings = false;
    bool showAerialMovementSettings = false;

	private void OnEnable() {
        cc = serializedObject.FindProperty( "cc" );
        cam = serializedObject.FindProperty( "cam" );

        inverted = serializedObject.FindProperty( "inverted" );
        sensitivity = serializedObject.FindProperty( "sensitivity" );
        xRotationLimitsUp = serializedObject.FindProperty( "xRotationLimitsUp" );
        xRotationLimitsDown = serializedObject.FindProperty( "xRotationLimitsDown" );

        headBob = serializedObject.FindProperty( "headBob" );
        headBobSpeed = serializedObject.FindProperty( "headBobSpeed" );
        headBobIntensity = serializedObject.FindProperty( "headBobIntensity" );

        FOVBoostDuration = serializedObject.FindProperty( "FOVBoostDuration" );
        FOVBoostIntensity = serializedObject.FindProperty( "FOVBoostIntensity" );
        FOVBoostCurve = serializedObject.FindProperty( "FOVBoostCurve" );

        maximumTilt = serializedObject.FindProperty( "maximumTilt" );

        speed = serializedObject.FindProperty( "speed" );
        stickToGroundForce = serializedObject.FindProperty( "stickToGroundForce" );
        startSprintCurve = serializedObject.FindProperty( "startSprintCurve" );
        endSprintCurve = serializedObject.FindProperty( "endSprintCurve" );

        groundDistance = serializedObject.FindProperty( "groundDistance" );
        groundMask = serializedObject.FindProperty( "groundMask" );

        jumpHeight = serializedObject.FindProperty( "jumpHeight" );
        jumps = serializedObject.FindProperty( "jumps" );
        gravity = serializedObject.FindProperty( "gravity" );

        sprintAllowed = serializedObject.FindProperty( "sprintAllowed" );
        FOVBurst = serializedObject.FindProperty( "FOVBurst" );
        FOVBoost = serializedObject.FindProperty( "FOVBoost" );
        sprintSpeed = serializedObject.FindProperty( "sprintSpeed" );
        relativeSprintSpeed = serializedObject.FindProperty( "relativeSprintSpeed" );

        toggleCrouch = serializedObject.FindProperty( "toggleCrouch" );
        crouchCamHeight = serializedObject.FindProperty( "crouchCamHeight" );
        crouchPlayerHeight = serializedObject.FindProperty( "crouchPlayerHeight" );
        crouchSpeed = serializedObject.FindProperty( "crouchSpeed" );
        relativeCrouchSpeed = serializedObject.FindProperty( "relativeCrouchSpeed" );

        aerialMovement = serializedObject.FindProperty( "aerialMovement" );
        aerialTurnSpeed = serializedObject.FindProperty( "aerialTurnSpeed" );
    }

	public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField( cc );
        EditorGUILayout.PropertyField( cam );

        GUIStyle style = GUI.skin.GetStyle("FoldoutHeader");

        showCameraSettings = EditorGUILayout.Foldout( showCameraSettings, "Camera Settings", style );
        if ( showCameraSettings ) {
            EditorGUILayout.PropertyField( inverted );
            EditorGUILayout.PropertyField( sensitivity );
            EditorGUILayout.PropertyField( xRotationLimitsUp );
            EditorGUILayout.PropertyField( xRotationLimitsDown );

            EditorGUI.indentLevel++;
            showHeadBobSettings = EditorGUILayout.Foldout( showHeadBobSettings, "Head Bob Settings", style );
            if ( showHeadBobSettings ) {
                EditorGUILayout.PropertyField( headBob );
                EditorGUILayout.PropertyField( headBobSpeed );
                EditorGUILayout.PropertyField( headBobIntensity );
            }

            showFOVBoostSettings = EditorGUILayout.Foldout( showFOVBoostSettings, "FOV Boost Settings", style );
            if ( showFOVBoostSettings ) {
                EditorGUILayout.PropertyField( FOVBoostDuration );
                EditorGUILayout.PropertyField( FOVBoostIntensity );
                EditorGUILayout.PropertyField( FOVBoostCurve );
            }

            ShowScreentiltSettings = EditorGUILayout.Foldout( ShowScreentiltSettings, "Screen Tilt Settings", style );
            if ( ShowScreentiltSettings ) {
                EditorGUILayout.PropertyField( maximumTilt );
            }
            EditorGUI.indentLevel--;
        }

        showPlayerSettings = EditorGUILayout.Foldout( showPlayerSettings, "Player Settings", style );
		if ( showPlayerSettings ) {
            EditorGUILayout.PropertyField( speed );
            EditorGUILayout.PropertyField( gravity );
            EditorGUILayout.PropertyField( stickToGroundForce );

            EditorGUI.indentLevel++;
            showGroundCheckSettings = EditorGUILayout.Foldout( showGroundCheckSettings, "GroundCheck Settings", style );
            if ( showGroundCheckSettings ) {
                EditorGUILayout.PropertyField( groundDistance );
                EditorGUILayout.PropertyField( groundMask );
            }

            showJumpSettings = EditorGUILayout.Foldout( showJumpSettings, "Jump Settings", style );
            if ( showJumpSettings ) {
                EditorGUILayout.PropertyField( jumpHeight );
                EditorGUILayout.PropertyField( jumps );
            }

            showSprintSettings = EditorGUILayout.Foldout( showSprintSettings, "Sprint Settings", style );
            if ( showSprintSettings ) {
                EditorGUILayout.PropertyField( sprintAllowed );
                EditorGUILayout.PropertyField( FOVBurst );
                EditorGUILayout.PropertyField( FOVBoost );
                EditorGUILayout.PropertyField( sprintSpeed );
                EditorGUILayout.PropertyField( relativeSprintSpeed );
                EditorGUILayout.PropertyField( startSprintCurve );
                EditorGUILayout.PropertyField( endSprintCurve );
            }

            showCrouchSettings = EditorGUILayout.Foldout( showCrouchSettings, "Crouch Settings", style );
            if ( showCrouchSettings ) {
                EditorGUILayout.PropertyField( toggleCrouch );
                EditorGUILayout.PropertyField( crouchCamHeight );
                EditorGUILayout.PropertyField( crouchPlayerHeight );
                EditorGUILayout.PropertyField( crouchSpeed );
                EditorGUILayout.PropertyField( relativeCrouchSpeed );
            }

            showAerialMovementSettings = EditorGUILayout.Foldout( showAerialMovementSettings, "Aerial Movement Settings", style );
            if ( showAerialMovementSettings ) {
                EditorGUILayout.PropertyField( aerialMovement );
                EditorGUILayout.PropertyField( aerialTurnSpeed );
            }
        }

        serializedObject.ApplyModifiedProperties();
	}
}
