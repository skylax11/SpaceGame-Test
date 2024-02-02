using Assets.Scripts;
using Assets.Scripts.Enum;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Controller : MonobehaviourSingleton<Controller>
{
    [Header("Character Movement Properties")]
    public float MoveSpeed = 5f;
    public float CrouchSpeed = 3f;
    public float RunSpeed = 7.5f;
    public float DefaultRunSpeed = 7.5f;   // Declaring two different RunSpeed variables (RunSpeed , DefaultRunSpeed) for changing running speed while
    public float CrouchRunSpeed = 5.5f;    // crouching and saving old RunSpeed value.
    public float TotalSpeed = 0f;
    public float JumpForce = 6f;
    private Vector3 _direction;
    public MovementState Movement;

    [Header("Layer For Mouse")]
    public LayerMask layer;
    [Header("Animation")]
    public Animator m_AnimationController;
    public float m_Speed;
    [Header("Rigidbody")]
    [SerializeField] Rigidbody m_Rigidbody;
    [Header("UI")]
    [SerializeField] UI_Manager m_UIManager;
  
    private void Start() => transform.rotation = Quaternion.Euler(0, 180, 0);

    private void Update()
    {
        if (Movement == MovementState.Moving)
        {
            TotalSpeed = Mathf.Lerp(TotalSpeed, MoveSpeed, Time.deltaTime * 5f);
        }
        else if (Movement == MovementState.Running)
        {
            TotalSpeed = Mathf.Lerp(TotalSpeed, RunSpeed, Time.deltaTime * 5f);
        }
        else if(Movement == MovementState.Standing)
        {
            TotalSpeed = Mathf.Lerp(TotalSpeed, 0, Time.deltaTime * 5f);
        }
        else if (Movement == MovementState.Crouching)
        {
            TotalSpeed = Mathf.Lerp(TotalSpeed, CrouchSpeed, Time.deltaTime * 5f);
        }
        Move();
        RotateMouse();
    }
    public void RotateMouse()
    {
        _direction = new Vector3(PlayerInputManager.Instance.direction.x, 0, PlayerInputManager.Instance.direction.y);
        SetWalkingAnimations();
        
        SetDirection();
        Ray lookAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(lookAt, out hitInfo, Mathf.Infinity, layer))
        {
            if (layer == LayerMask.GetMask("Ground"))
            {
                var relativePos = transform.position - hitInfo.point;
                var rotation = Quaternion.LookRotation(-relativePos).eulerAngles;
                transform.rotation = Quaternion.Euler(0, rotation.y, 0);
            }
            else
                return;
        }

    }
    private float _directionX = 0f;
    private float _directionY = 0f;
    public void SetWalkingAnimations()
    {
        _directionX = Mathf.Lerp(_directionX, _direction.x, Time.deltaTime*12f);
        _directionY = Mathf.Lerp(_directionY, _direction.z, Time.deltaTime*12f);

        m_AnimationController.SetFloat("horizontal", m_Speed * _directionX);
        m_AnimationController.SetFloat("vertical",   m_Speed * _directionY);
    }
    public void SetDirection()
    {
        if (_direction == new Vector3(0, 0, 1)) 
            _direction = transform.forward;
        else if (_direction == new Vector3(0, 0, -1))
            _direction = -transform.forward;
        else if (_direction == new Vector3(1, 0, 0))
            _direction = transform.right;
        else if (_direction == new Vector3(-1, 0, 0))
            _direction = -transform.right;
        else if (_direction == new Vector3(-1, 0, 1))
            _direction = -transform.right + transform.forward;
        else if (_direction == new Vector3(-1, 0, -1))
            _direction = -transform.right - transform.forward;
        else if (_direction == new Vector3(1, 0, 1))
            _direction = transform.right + transform.forward;
        else if (_direction == new Vector3(1, 0, -1))
            _direction = transform.right - transform.forward;
    }

    public void Move()
    {
        m_Rigidbody.velocity = Vector3.Slerp(m_Rigidbody.velocity,_direction*TotalSpeed,Time.deltaTime*15f);
        m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity,TotalSpeed);  
    }                                                                                     
}
