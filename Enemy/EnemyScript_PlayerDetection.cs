using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class EnemyScript_PlayerDetection : MonoBehaviour
    {
        [SerializeField] EnemyScript m_EnemyScript;
        [Header("View")]
        public float radius;
        public float angle;
        public LayerMask PlayerMask;
        public LayerMask ObstructionMask;
        public bool canSeePlayer;
        private void Update()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius,PlayerMask);
            if (rangeChecks.Length > 0)
            {
                Transform target = rangeChecks[0].transform;
                Vector3 direction = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward,direction) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, direction, Mathf.Infinity, ObstructionMask))
                        m_EnemyScript.isPlayerDetected = true;
                    else
                        m_EnemyScript.isPlayerDetected = false;
                }
                else
                    m_EnemyScript.isPlayerDetected = false;

            }
            else if (m_EnemyScript.isPlayerDetected)
                m_EnemyScript.isPlayerDetected = false;
            
        }

    }
}
