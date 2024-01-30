using Assets.Scripts.Character_Scripts.Inventory;
using Assets.Scripts.Scriptable_Object;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class SetRigSettings : MonobehaviourSingleton<SetRigSettings>
{
    public bool LetSetRigs;
    public Rig _aimingRig;
    public Rig _holdingRig;
    public RigBuilder rigBuild;
    [SerializeField] GameObject AimPose;
    public MultiPositionConstraint MultiPositionConstraint_Stand;
    public MultiPositionConstraint MultiPositionConstraint_Aim;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Left;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Right;

    public Transform LeftHint;
    public Transform RightHint;

    [SerializeField] Riggings _aimingRiggings;
    [SerializeField] Riggings _holdingRiggings;
    public Transform LeftHand;
    public Transform RightHand;
    [SerializeField] Transform RotateChest;
    private GameObject _currentWeapon;
    [SerializeField] Weapon_SO _currentSO;
    [SerializeField] WeaponController CurrentWeapon;
    void Start() => SetNewWeaponSettings();
    public void BuildRig() => rigBuild.Build();

    public void SetNewWeaponSettings()
    {
        if (GetComponent<SlotSystem>().CurrentSlot.gameObject == null)
        {
            this.enabled = false;
            return;
        }
        else
            this.enabled = true;

        _currentWeapon = GetComponent<SlotSystem>().CurrentSlot.gameObject;
        _currentSO = _currentWeapon.GetComponent<Weapon>().WeaponSO;
    }
    void Update()
    {       
        if (LetSetRigs)
        {
            AimPose.transform.localRotation = Quaternion.Slerp(AimPose.transform.localRotation, Quaternion.Euler(_currentSO.WeaponAimRotate.x, _currentSO.WeaponAimRotate.y, _currentSO.WeaponAimRotate.z), Time.deltaTime * 15f);
            _aimingRig.weight += Time.deltaTime * 20f;
            MultiPositionConstraint_Aim.data.offset = Vector3.Slerp(MultiPositionConstraint_Aim.data.offset,CurrentWeapon.weapon.WeaponSO.WeaponAimMultiPos_Offsets,Time.deltaTime*15f);
            LeftHand.localPosition  = Vector3.Slerp(LeftHand.localPosition, _aimingRiggings._leftPos, Time.deltaTime * 15f);
            RightHand.localPosition = Vector3.Slerp(RightHand.localPosition, _aimingRiggings._rightPos, Time.deltaTime * 15f);
            LeftHand.localRotation  = Quaternion.Slerp(LeftHand.localRotation, Quaternion.Euler(_aimingRiggings._leftRot.x, _aimingRiggings._leftRot.y, _aimingRiggings._leftRot.z), Time.deltaTime * 15f);
            RightHand.localRotation = Quaternion.Slerp(RightHand.localRotation, Quaternion.Euler(_aimingRiggings._rightRot.x, _aimingRiggings._rightRot.y, _aimingRiggings._rightRot.z), Time.deltaTime * 15f);
            RotateChest.localRotation = Quaternion.Slerp(RotateChest.localRotation, Quaternion.Euler(4f, RotateChest.localRotation.y, RotateChest.localRotation.z), Time.deltaTime * 5f);
        }
        else
        {
            AimPose.transform.localRotation = Quaternion.Slerp(AimPose.transform.localRotation, Quaternion.Euler(_currentSO.WeaponIKRot.x, _currentSO.WeaponIKRot.y, _currentSO.WeaponIKRot.z), Time.deltaTime * 15f);
            _aimingRig.weight -= Time.deltaTime * 12f;
            LeftHand.localPosition = Vector3.Slerp(LeftHand.localPosition, _holdingRiggings._leftPos, Time.deltaTime * 15f);
            RightHand.localPosition = Vector3.Slerp(RightHand.localPosition, _holdingRiggings._rightPos, Time.deltaTime * 15f);
            LeftHand.localRotation = Quaternion.Slerp(LeftHand.localRotation, Quaternion.Euler(_holdingRiggings._leftRot.x, _holdingRiggings._leftRot.y, _holdingRiggings._leftRot.z), Time.deltaTime * 15f);
            RightHand.localRotation = Quaternion.Slerp(RightHand.localRotation, Quaternion.Euler(_holdingRiggings._rightRot.x, _holdingRiggings._rightRot.y, _holdingRiggings._rightRot.z), Time.deltaTime * 15f);
            RotateChest.localRotation = Quaternion.Slerp(RotateChest.localRotation, Quaternion.Euler(-9f, RotateChest.localRotation.y, RotateChest.localRotation.z), Time.deltaTime * 5f);
        }
    }
    public void SetRigDatas(Weapon weapon)
    {
        _currentWeapon = weapon.gameObject;
        SetNewWeaponSettings();
        CurrentWeapon.bulletTransform = weapon.BulletPos;
        CurrentWeapon.weapon = weapon;

        _holdingRiggings._leftPos = weapon.WeaponSO.LeftHandPosePos;
        _holdingRiggings._leftRot = weapon.WeaponSO.LeftHandPoseRot;
        _holdingRiggings._rightPos = weapon.WeaponSO.RightHandPosePos;
        _holdingRiggings._rightRot = weapon.WeaponSO.RightHandPoseRot;

        _aimingRiggings._leftPos = weapon.WeaponSO.LeftHandAimPos;
        _aimingRiggings._leftRot = weapon.WeaponSO.LeftHandAimRot;
        _aimingRiggings._rightPos = weapon.WeaponSO.RightHandAimPos;
        _aimingRiggings._rightRot = weapon.WeaponSO.RightHandAimRot;

        TwoBoneConstraintIK_Left.data.target = weapon.LeftHand.transform;
        TwoBoneConstraintIK_Right.data.target = weapon.RightHand.transform;

        LeftHand = weapon.LeftHand.transform;
        RightHand = weapon.RightHand.transform;
        MultiPositionConstraint_Stand.data.offset = weapon.WeaponSO.WeaponPoseMultiPos_Offsets;
        MultiPositionConstraint_Aim.data.offset = weapon.WeaponSO.WeaponAimMultiPos_Offsets;

        TwoBoneConstraintIK_Left.data.targetPositionWeight = weapon.WeaponSO.LeftHandWeight;
        TwoBoneConstraintIK_Right.data.targetPositionWeight = weapon.WeaponSO.RightHandWeight;

        LeftHint.localPosition = weapon.WeaponSO.LeftHintPos;
        RightHint.localPosition = weapon.WeaponSO.RightHintPos;

        BuildRig();
    }
    public void ResetRigDatas(Weapon weapon)
    {
        CurrentWeapon.weapon = weapon;

        TwoBoneConstraintIK_Left.data.target  = null;
        TwoBoneConstraintIK_Right.data.target = null;

        BuildRig();
    }
}
