using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IHuman
    {
        public int Health { get; set; }
        public void TakeDamage(int damage);
    }
}
