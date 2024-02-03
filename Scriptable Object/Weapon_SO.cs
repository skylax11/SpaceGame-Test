using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Scriptable_Object
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/Create Item")]
    public class Weapon_SO : ScriptableObject
    {
        public new string Name;
        public Vector3 WeaponPose;
        public Vector3 WeaponRot;
        public Vector3 WeaponPivot_Pos;
        public Vector3 WeaponPivot_Rot;
        public Vector3 WeaponPoseMultiPos_Offsets;
        public Vector3 WeaponAimMultiPos_Offsets;
        public Vector3 WeaponAimRotate;
        public Vector3 LeftHandPoseRot;
        public Vector3 LeftHandPosePos;
        public Vector3 RightHandPoseRot;
        public Vector3 RightHandPosePos;
        public Vector3 LeftHandAimRot;
        public Vector3 LeftHandAimPos;
        public Vector3 RightHandAimRot;
        public Vector3 RightHandAimPos;
        public float LeftHandWeight;
        public float RightHandWeight;
        public Vector3 WeaponSize;
        public Vector3 WeaponIKRot;
        public Vector3 LeftHintPos;
        public Vector3 RightHintPos;
        public Vector3 LeftHintPosAim;
        public Vector3 RightHintPosAim;
        public GameObject BulletBox;
        public AudioClip WeaponShot;
        public AudioClip WeaponReload;
    }
}
