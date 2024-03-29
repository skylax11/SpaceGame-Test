using Assets.Scripts;
using Assets.Scripts.Enemy;
using Assets.Scripts.Enum;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyScript : MonoBehaviour , IHuman
{
    [Header("Effects")]
    [SerializeField] DissolveEffect m_dissolveEffect;
    [SerializeField] DissappearEffect m_PlayerDissappearEffect;

    [Header("Enemy Scripts")]
    [SerializeField] EnemyScript_PlayerDetection m_Detection;
    [SerializeField] EnemyScript_Rigs m_Rigs;
    [SerializeField] int _health;
    public bool callOnce = true;

    [Header("Player")]
    [SerializeField] Controller Player;
    public bool isPlayerDetected;
    public float MinDistance;
    public float MaxDistance;
    public GameObject Weapon_GO;

    [Header("Object Pooling")]
    [SerializeField] Transform TransformHierarchy;
    [SerializeField] GameObject BulletPrefab;
    [SerializeField] Transform BulletDirection;

    [Header("Weapon")]
    public float FireFreq;
    public Vector3 GunScatter;
    private float _fireCounter;
    private static Queue<GameObject> _bullets = new Queue<GameObject>();


    [Header("NavMesh Agent")]
    public NavMeshAgent navMeshAgent;
    public Vector3 StartPoint;
    public bool gotShot = false;
    public bool moveOn = false;
    public bool canShot = true;
    [Header("Strafe Properties")]
    public Transform StrafeLeft;
    public Transform StrafeRight;
    public bool isGoingLeft;
    public bool isGoingRight;
    public bool CanStrafe;

    [Header("Patrol")]
    public bool canPatrol;
    [SerializeField] Enemy_Patrol m_EnemyPatrol;

    [Header("Movement Situation")]
    public MovementState Movement;
    public float RunSpeed  = 7f;
    public float WalkSpeed = 5f;
    public float TotalSpeed;

    [Header("Animation")]
    [SerializeField] Animator m_Animator;

    [Header("Audio Source")]
    [SerializeField] AudioSource m_AudioSource;

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
    private Controller m_PlayerController;
    private Character m_Character;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_PlayerController = Player.GetComponent<Controller>();
        m_Character = Player.GetComponentInChildren<Character>();
        m_PlayerDissappearEffect = Player.GetComponent<DissappearEffect>();
    }
    private void Update()
    {
        if (Movement == MovementState.Moving)
            TotalSpeed = Mathf.Lerp(TotalSpeed, WalkSpeed, Time.deltaTime * 5f);
        else if (Movement == MovementState.Running)
            TotalSpeed = Mathf.Lerp(TotalSpeed, RunSpeed, Time.deltaTime * 5f);

        if (m_PlayerDissappearEffect.isVisible == false)
        {
            ResetAllBehaviour();
            return;
        }

        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (m_Character.Health > 0 && (gotShot || ((distance < MaxDistance) && (m_PlayerController.Movement == MovementState.Moving || m_PlayerController.Movement == MovementState.Running))))
            moveOn = true;
        else
            moveOn = false;

        if (isPlayerDetected || moveOn)
        {
            callOnce = true;

            if (distance > MaxDistance)
            {
                ResetAllBehaviour();
                return;
            }
            if (distance > MinDistance)
                Chase();
            else
            {
                StopChasing();
                if (CanStrafe && canShot)
                    Strafe();
            }
            RealizeEnemy();
        }
        else
        {
             if (!callOnce)
                 return;
            ResetAllBehaviour();
            callOnce = false;
        }
    }
    public void DoPatrol()
    {
        m_Animator.SetBool("ComeBack", true);
        m_EnemyPatrol.DoPatrol();
        navMeshAgent.isStopped = false;
    }
    public void RealizeEnemy()
    {
        LookPlayer();

        if (canShot)
        {
            m_Rigs.enableRig = true;
            Shot();
            m_Animator.SetBool("RealizedEnemy", true);
        }
        else
        {
            m_Rigs.enableRig = false;
            m_Animator.SetBool("RealizedEnemy", false);
            Chase();
        }

    }
    public void ResetAllBehaviour()
    {
        gotShot = false;
        m_Rigs.enableRig = false;
        moveOn = false;
        ResetStrafe();
        StopChasing();
        if (!canPatrol)
            InvokeRepeating("ComeBack", 0.1f, 0.5f);
        else
            DoPatrol();
        m_Animator.SetBool("RealizedEnemy", false);
    }
    public void ComeBack()
    {
        Movement = MovementState.Moving;
        navMeshAgent.isStopped = false;
        navMeshAgent.stoppingDistance = 0;
        navMeshAgent.speed = TotalSpeed;
        navMeshAgent.SetDestination(StartPoint);
        
        if (Vector3.Distance(transform.position, StartPoint) <= 1)
        {
            m_Animator.SetBool("ComeBack", false);
            CancelInvoke("ComeBack");
        }
        else
            m_Animator.SetBool("ComeBack", true);
    }
    public void DoStrafe()
    {
        navMeshAgent.isStopped = true;
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
        transform.Translate(new Vector3(Time.deltaTime*temp, 0, 0) * TotalSpeed);

    }
    public void Strafe()
    {
        if (!Physics.Raycast(StrafeLeft.transform.position, -StrafeLeft.transform.right, 0.1f) && isGoingLeft)
            isGoingLeft = true;
        else
        {
            isGoingLeft = false;
            isGoingRight = true;
        }
        if (!Physics.Raycast(StrafeRight.transform.position, StrafeRight.transform.right, 0.1f) && isGoingRight)
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
        isGoingLeft = false;
        isGoingRight = false;
    }
    public void Chase()
    {
        m_Animator.SetBool("taunt", false);
        Movement = MovementState.Running;
        navMeshAgent.isStopped = false;
        ResetStrafe();
        m_Animator.SetBool("StartChasing", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.stoppingDistance = MinDistance;
        navMeshAgent.speed = TotalSpeed;
        navMeshAgent.SetDestination(Player.transform.position);
    }
    public void StopChasing()
    {
        Movement = MovementState.Moving;
        if(!canPatrol)
            navMeshAgent.stoppingDistance = MinDistance;
        else
            navMeshAgent.stoppingDistance = 0;
        m_Animator.SetBool("StartChasing", false);
    }
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
            if ((m_AudioSource.time < m_AudioSource.clip.length / 1.2))
                m_AudioSource.Play();
            Static_ObjectPooling.do_ObjectPooling(this,BulletPrefab, TransformHierarchy, BulletDirection, _bullets);
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
        if (!isPlayerDetected)
            gotShot = true;
    }
    private Rigidbody spine;
    public void Death(Vector3 hitDirection)
    {
        navMeshAgent.enabled = false;
        m_Detection.enabled = false;
        isPlayerDetected = false;
        moveOn = false;

        var RigidbodyWep = Weapon_GO.GetComponent<Rigidbody>();
        RigidbodyWep.isKinematic = false;
        RigidbodyWep.useGravity = true;
        RigidbodyWep.velocity = Vector3.zero;
        RigidbodyWep.AddForce(new Vector3(0f, 0f, -hitDirection.normalized.z), ForceMode.Force);

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

        this.enabled = false; // disabling script
    }
    IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponentInParent<Animator>().enabled = false;
    }
    public void DisableConstraints() => GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
}
