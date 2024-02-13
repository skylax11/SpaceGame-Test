using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Enemy
{
    public class EnemyScript_PlayerDetection : MonoBehaviour
    {
        [Header("Enemy Script")]
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
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (Vector3.Angle(transform.forward, direction) < angle / 2)
                    CheckForObstacle(target, distanceToTarget);
                else
                    SetBooleans(false);
            }
            else if (m_EnemyScript.isPlayerDetected)
                SetBooleans(false);
        }
        private void SetBooleans(bool s) => m_EnemyScript.canShot = m_EnemyScript.isPlayerDetected = s;
        private void CheckForObstacle(Transform target,float distanceToTarget)
        {
            if (!Physics.Raycast(transform.position, target.position - transform.position, distanceToTarget, ObstructionMask))
                SetBooleans(true);
            else
                SetBooleans(false);
        }
    }
}
