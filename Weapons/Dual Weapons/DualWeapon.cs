﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.Weapons.Dual_Weapons
{
    public class DualWeapon : Weapon
    {
        [SerializeField] public Transform BulletLeft;
        [SerializeField] public Transform BulletRight;
        public override void Fire(Weapon weapon, GameObject prefab, Transform bulletHierarchy, Queue<GameObject> bullets)
        {
            DualWeapon dualWeapon = weapon as DualWeapon;
            Static_ObjectPooling.do_ObjectPooling(weapon, prefab, bulletHierarchy, dualWeapon.BulletLeft, bullets);
            Static_ObjectPooling.do_ObjectPooling(weapon, prefab, bulletHierarchy, dualWeapon.BulletRight, bullets);
            Ammo--;
        }
        public override GameObject ReloadAnimation(GameObject newMagazine, GameObject Magazine)
        {
            Magazine.SetActive(false);
            GameObject income = Instantiate(newMagazine, null);
            income.transform.localScale = new Vector3(2f, 2f, 2f);
            income.transform.parent = null;
            income.transform.position = Magazine.transform.position;
            income.transform.rotation = Magazine.transform.rotation;

            var RigidBody = income.transform.GetComponent<Rigidbody>();
            var Collider = income.transform.GetComponents<Collider>();

            Collider[0].enabled = false;
            Collider[1].enabled = false;
            RigidBody.useGravity = true;
            RigidBody.isKinematic = false;
            RigidBody.isKinematic = false;

            Rigidbody[] twoWeapon = income.transform.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < twoWeapon.Length; i++)
            {
                var ChildRigidbody = twoWeapon[i].transform.GetComponent<Rigidbody>();
                twoWeapon[i].transform.parent = null;
                ChildRigidbody.isKinematic = false;
                ChildRigidbody.useGravity = true;
                twoWeapon[i].transform.GetComponent<Collider>().enabled = true;    
                ChildRigidbody.AddForce(transform.forward.normalized * 100f * Mathf.Pow(-1,i));
            }
            return income;
        }
        public override void SetReload(bool situation) => AnimationController.Instance.SetAnimation("Reload2", situation);

        public override void ThrowWeaponAway()
        {
            transform.parent = null;

            var Rigidbodies = GetComponentsInChildren<Rigidbody>().Where(x => x.TryGetComponent(out RigTransform uzi) != false).ToList();
            var Colliders   = GetComponentsInChildren<Collider>().Where(x=>x.TryGetComponent(out RigTransform uzi) != false).ToList();
            
            for (int i = 0; i < 2; i++)
            {
                Colliders[i].transform.parent = null;
                Rigidbodies[i].useGravity = true;
                Rigidbodies[i].isKinematic = false;
                Rigidbodies[i].AddForce(gameObject.transform.forward * 100f * Mathf.Pow(-1,i));
                Colliders[i].enabled = true;
            }
        }
    }
}
