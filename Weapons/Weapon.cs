using Assets.Scripts.Scriptable_Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Scripts.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public Weapon_SO WeaponSO;
        public float FireFreq;
        public float Damage;
        public int Ammo;
        public int Magazine;
        public int FullAmmo;
        public GameObject BulletBox;
        public GameObject SpotLight;
        public GameObject LeftHand;
        public GameObject RightHand;
        public Transform BulletPos;
        public bool isDualWeapon;
        public bool isReadyToUse;
        public void Reload()
        {
            int neededAmmo = FullAmmo - Ammo;
            if (neededAmmo <= Magazine)
            {
                Magazine -= neededAmmo;
                Ammo += neededAmmo;
            }
            else
            {
                Ammo += Magazine;
                Magazine = 0;
            }
        }
        public abstract void Fire(Weapon weapon, GameObject prefab, Transform bulletHierarchy, Queue<GameObject> bullets);
        public abstract GameObject ReloadAnimation(GameObject newMagazine,GameObject Magazine);
        public abstract void SetReload(bool situation);
        public abstract void ThrowWeaponAway();
    }
}
