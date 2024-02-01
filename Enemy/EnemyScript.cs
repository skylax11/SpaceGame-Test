using Assets.Scripts;
using Assets.Scripts.Enemy;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour , IHuman
{
    [SerializeField] EnemyScript_DissolveEffect m_dissolveEffect;
    [SerializeField] EnemyScript_PlayerDetection m_Detection;
    [SerializeField] EnemyScript_Rigs m_Rigs;
    [SerializeField] Controller Player;
    [SerializeField] int _health;
    [SerializeField] Animator m_Animator;
    public bool isPlayerDetected;
    public float MinDistance;
    public float MaxDistance;
    [Header("Object Pooling")]
    [SerializeField] Transform TransformHierarchy;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletDirection; 
    private static Queue<GameObject> _bullets = new Queue<GameObject>();
    public float FireFreq;
    private float _fireCounter;
    [Header("NavMesh Agent")]
    public NavMeshAgent navMeshAgent;
    [Header("Strafe Properties")]
    public Transform StrafeLeft;
    public Transform StrafeRight;
    public bool isGoingLeft;
    public bool isGoingRight;
    public bool CanStrafe;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    private void Update()
    {
        if (isPlayerDetected)
        {
            if (Vector3.Distance(transform.position, Player.transform.position) > MaxDistance)
            {
                ResetAllBehaviour();
                return;
            }

            navMeshAgent.isStopped = false;

            if (Vector3.Distance(transform.position, Player.transform.position) > MinDistance)
                Chase();
            else
            {
                StopChasing();
                if(CanStrafe)
                Strafe();
            }

            m_Rigs.enableRig = true;
            LookPlayer();
            Shot();
            m_Animator.SetBool("RealizedEnemy", true);
        }
        else
        {
            m_Rigs.enableRig = false;
            m_Animator.SetBool("RealizedEnemy", false);
        }
    }
    public void ResetAllBehaviour()
    {
        m_Rigs.enableRig = false;
        ResetStrafe();
        StopChasing();
        m_Animator.SetBool("RealizedEnemy", false);
        navMeshAgent.isStopped = true;
    }
    public void DoStrafe()
    {
        int temp = 1;
        if (!isGoingLeft)
        {
            m_Animator.SetBool("RightStrafe", true);
            m_Animator.SetBool("LeftStrafe", false);
            temp = 1;
        }
        else
        {
            m_Animator.SetBool("LeftStrafe", true);
            m_Animator.SetBool("RightStrafe", false);
            temp = -1;
        }
        transform.Translate(new Vector3(Time.deltaTime*temp, 0, 0) * 2f);

    }
    public void Strafe()
    {
        if (!Physics.Raycast(StrafeLeft.transform.position, -StrafeLeft.transform.right,1f) && isGoingLeft)
            isGoingLeft = true;
        else
        {
            isGoingLeft = false;
            isGoingRight = true;
        }
        if (!Physics.Raycast(StrafeRight.transform.position, StrafeRight.transform.right, 1f) && isGoingRight)
            isGoingLeft = false;
        else
        {
            isGoingLeft = true;
            isGoingRight = false;
        }
        DoStrafe();

    }
    public void ResetStrafe()
    {
        m_Animator.SetBool("RightStrafe", false);
        m_Animator.SetBool("LeftStrafe", false);
    }
    public void Chase()
    {
        ResetStrafe();
        m_Animator.SetBool("StartChasing", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(Player.transform.position);
    }
    public void StopChasing() => m_Animator.SetBool("StartChasing", false);

    public void LookPlayer()
    {
        var relativePos = transform.position - Player.transform.position;
        var rotation = Quaternion.LookRotation(-relativePos).eulerAngles;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, rotation.y, 0), Time.deltaTime * 15f);
    }
    public void Shot()
    {
        if (Time.time > _fireCounter)
        {
            _fireCounter = FireFreq + Time.time;
            Static_ObjectPooling.do_ObjectPoolingEnemy(BulletPrefab, TransformHierarchy, BulletDirection, _bullets);
        }
    }
    public void TakeDamage(int damage,Vector3 hitDirection)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Death(hitDirection);
            m_dissolveEffect.DoDissolving();
        }
    }
    private Rigidbody spine;
    public void Death(Vector3 hitDirection)
    {
        navMeshAgent.enabled = false;
        m_Detection.enabled = false;
        isPlayerDetected = false;
        GetComponent<Collider>().enabled = false;
        m_Animator.updateMode = AnimatorUpdateMode.Normal;
        DisableConstraints();
        foreach (var Rigidbody_Parts in GetComponentsInChildren<Rigidbody>())
        {
            Rigidbody_Parts.isKinematic = false;
            Rigidbody_Parts.useGravity  = true;
            if (Rigidbody_Parts.transform.CompareTag("Spine"))
                spine = Rigidbody_Parts;
        }
        foreach (var Collider in GetComponentsInChildren<Collider>())
            Collider.enabled = true;
        StartCoroutine("DisableAnimator");
        spine.AddForce(new Vector3(0f,0f,-hitDirection.normalized.z) * 50f, ForceMode.Impulse);
    }
    IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponentInParent<Animator>().enabled = false;
    }
    public void DisableConstraints() => GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

}
