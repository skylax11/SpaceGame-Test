using Assets.Scripts.Weapons.Dual_Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons
{
    public class SpaceRifle : Weapon
    {
        public override void Fire(Weapon weapon, GameObject prefab, Transform bulletHierarchy, Queue<GameObject> bullets)
        {
            Static_ObjectPooling.do_ObjectPooling(weapon, prefab, bulletHierarchy, weapon.BulletPos, bullets);
            Ammo--;
        }
        public override GameObject ReloadAnimation(GameObject newMagazine, GameObject Magazine)
        {
            Magazine.SetActive(false);
            GameObject income = Instantiate(newMagazine, null);

            foreach(var Collider in IgnoreCollision)
                Physics.IgnoreCollision(income.GetComponent<Collider>(), Collider); // disabling collision between
                                                                                    // character and the bullet box
            income.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            income.transform.parent = null;
            income.transform.position = Magazine.transform.position;
            income.transform.rotation = Magazine.transform.rotation;
            income.transform.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * 200f);
            return income;
        }
        public override void SetReload(bool situation) => AnimationController.Instance.SetAnimation("Reload", situation);
        public override void ThrowWeaponAway()
        {
            transform.parent = null;

            var Rigidbody = GetComponent<Rigidbody>();
            var Colliders = GetComponents<Collider>();

            Rigidbody.useGravity = true;
            Rigidbody.isKinematic = false;
            Rigidbody.AddForce(gameObject.transform.forward * 100f);

            Colliders[0].enabled = true;
            Colliders[1].enabled = true;
        }
    }
}
