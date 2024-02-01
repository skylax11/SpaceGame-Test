using Assets.Scripts.Scriptable_Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyScript_Rigs : MonoBehaviour
{
    [SerializeField] Weapon_SO Enemy_SO;
    public RigBuilder rigBuild;
    [SerializeField] public bool enableRig;
    [SerializeField] Transform LeftHand;
    [SerializeField] Transform RightHand;
    [SerializeField] Transform LeftHint;
    [SerializeField] Transform RightHint;
    [SerializeField] public Rig _aimingRig;
    [SerializeField] public Rig _holdingRig;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Left;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Right;

    void Update()
    {
        if (enableRig)
        {
            _aimingRig.weight += Time.deltaTime * 20f;
            LeftHand.localPosition = Vector3.Slerp(LeftHand.localPosition, Enemy_SO.LeftHandAimPos, Time.deltaTime * 15f);
            RightHand.localPosition = Vector3.Slerp(RightHand.localPosition, Enemy_SO.RightHandAimPos, Time.deltaTime * 15f);
            LeftHand.localRotation = Quaternion.Slerp(LeftHand.localRotation, Quaternion.Euler(Enemy_SO.LeftHandAimRot.x, Enemy_SO.LeftHandAimRot.y, Enemy_SO.LeftHandAimRot.z), Time.deltaTime * 15f);
            RightHand.localRotation = Quaternion.Slerp(RightHand.localRotation, Quaternion.Euler(Enemy_SO.RightHandAimRot.x, Enemy_SO.RightHandAimRot.y, Enemy_SO.RightHandAimRot.z), Time.deltaTime * 15f);
            LeftHint.localPosition = Vector3.Slerp(LeftHint.localPosition, Enemy_SO.LeftHintPosAim, Time.deltaTime * 15f);
            RightHint.localPosition = Vector3.Slerp(RightHint.localPosition, Enemy_SO.RightHintPosAim, Time.deltaTime * 15f);
        }
        else
        {
            _aimingRig.weight -= Time.deltaTime * 12f;
            LeftHand.localPosition = Vector3.Slerp(LeftHand.localPosition, Enemy_SO.LeftHandPosePos, Time.deltaTime * 15f);
            RightHand.localPosition = Vector3.Slerp(RightHand.localPosition, Enemy_SO.RightHandPosePos, Time.deltaTime * 15f);
            LeftHand.localRotation = Quaternion.Slerp(LeftHand.localRotation, Quaternion.Euler(Enemy_SO.LeftHandPoseRot.x, Enemy_SO.LeftHandPoseRot.y, Enemy_SO.LeftHandPoseRot.z), Time.deltaTime * 15f);
            RightHand.localRotation = Quaternion.Slerp(RightHand.localRotation, Quaternion.Euler(Enemy_SO.RightHandPoseRot.x, Enemy_SO.RightHandPoseRot.y, Enemy_SO.RightHandPoseRot.z), Time.deltaTime * 15f);
            LeftHint.localPosition = Vector3.Slerp(LeftHint.localPosition, Enemy_SO.LeftHintPos, Time.deltaTime * 15f);
            RightHint.localPosition = Vector3.Slerp(RightHint.localPosition, Enemy_SO.RightHintPos, Time.deltaTime * 15f);
        }
    }

}
