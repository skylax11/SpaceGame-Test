using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.VFX;
using UnityEngine;

namespace Assets.Scripts.Wall
{
    public class Metal : MonoBehaviour, IWall
    {
        private VisualEffect _effect;
        [SerializeField] GameObject _effectGameObjectPrefab;

        private void Start()
        {
            _effect = _effectGameObjectPrefab.GetComponent<VisualEffect>();
        }
        public VisualEffect Effect
        {
            get
            {
                return _effect;
            }
            set
            {
                _effect = value;
            }
        }
        public GameObject EffectGameObjectPrefab
        {
            get
            {
                return _effectGameObjectPrefab;
            }
            set
            {
                _effectGameObjectPrefab = value;
            }
        }

    }
}
