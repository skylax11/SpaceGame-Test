using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapons.Dual_Weapons
{
    public class DualWeapon : Weapon
    {
        [SerializeField] public Transform BulletLeft;
        [SerializeField] public Transform BulletRight;
    }
}
