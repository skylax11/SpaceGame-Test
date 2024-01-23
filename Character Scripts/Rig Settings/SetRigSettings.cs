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
    public RigBuilder rigBuild;
    [SerializeField] GameObject AimPose;
    public MultiPositionConstraint MultiPositionConstraint_Stand;
    public MultiPositionConstraint MultiPositionConstraint_Aim;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Left;
    public TwoBoneIKConstraint TwoBoneConstraintIK_Right;

    [SerializeField] Riggings _aimingRiggings;
    [SerializeField] Riggings _holdingRiggings;
    public Transform LeftHand;
    public Transform RightHand;
    [SerializeField] Transform RotateChest;
    public GameObject _currentWeapon;
    [SerializeField] Weapon_SO _currentSO;    
    void Start()
    {
        SetNewWeaponSettings();
    }
    public void SetNewWeaponSettings()
    {
        _currentSO = _currentWeapon.GetComponent<Weapon>().WeaponSO;
    }
    public void BuildRig() => rigBuild.Build();
    void Update()
    {
        if (LetSetRigs)
        {
          AimPose.transform.localRotation = Quaternion.Slerp(AimPose.transform.localRotation, Quaternion.Euler(_currentSO.WeaponAimRotate.x, _currentSO.WeaponAimRotate.y, _currentSO.WeaponAimRotate.z), Time.deltaTime * 15f);
          _aimingRig.weight += Time.deltaTime * 20f;
          LeftHand.localPosition  = Vector3.Slerp(LeftHand.localPosition, _aimingRiggings._leftPos, Time.deltaTime * 15f);
          RightHand.localPosition = Vector3.Slerp(RightHand.localPosition, _aimingRiggings._rightPos, Time.deltaTime * 15f);
          LeftHand.localRotation  = Quaternion.Slerp(LeftHand.localRotation, Quaternion.Euler(_aimingRiggings._leftRot.x, _aimingRiggings._leftRot.y, _aimingRiggings._leftRot.z), Time.deltaTime * 15f);
          RightHand.localRotation = Quaternion.Slerp(RightHand.localRotation, Quaternion.Euler(_aimingRiggings._rightRot.x, _aimingRiggings._rightRot.y, _aimingRiggings._rightRot.z), Time.deltaTime * 15f);
          RotateChest.localRotation = Quaternion.Slerp(RotateChest.localRotation, Quaternion.Euler(1f, RotateChest.localRotation.y, RotateChest.localRotation.z), Time.deltaTime * 5f);
        }
        else
        {
           AimPose.transform.localRotation = Quaternion.Slerp(AimPose.transform.localRotation, Quaternion.Euler(_currentSO.WeaponRot.x, _currentSO.WeaponRot.y, _currentSO.WeaponRot.z), Time.deltaTime * 15f);
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
            GetComponent<GunFireController>().bulletTransform = weapon.BulletPos;
            GetComponent<Controller>().CurrentWeapon = weapon;

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

        BuildRig();
    }
}
