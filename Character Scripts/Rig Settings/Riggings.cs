using Assets.Scripts.Character_Scripts.Inventory;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riggings : MonoBehaviour
{
    public Vector3 _leftPos;
    public Vector3 _rightPos;
    public Vector3 _leftRot;
    public Vector3 _rightRot;
    [SerializeField] bool isAiming;
    private void Start()
    {
       var weapon = GetComponentInParent<WeaponController>().weapon;
        if (weapon.name == "EmptyItem")
            return;
       if (!isAiming)
       {
           _leftPos  = weapon.WeaponSO.LeftHandPosePos;
           _leftRot  = weapon.WeaponSO.LeftHandPoseRot;
           _rightPos = weapon.WeaponSO.RightHandPosePos;
           _rightRot = weapon.WeaponSO.RightHandPoseRot;
            print("sa");
       }
       else
       {
           _leftPos  = weapon.WeaponSO.LeftHandAimPos;
           _leftRot  = weapon.WeaponSO.LeftHandAimRot;
           _rightPos = weapon.WeaponSO.RightHandAimPos;
           _rightRot = weapon.WeaponSO.RightHandAimRot;
            print("sa");

        }
    }
}   
