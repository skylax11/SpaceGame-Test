using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public interface IHuman
    {
        public int Health { get; set; }
        public void TakeDamage(int damage,Vector3 hitDirection);
    }
}
