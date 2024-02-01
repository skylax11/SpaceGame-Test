using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonobehaviourSingleton<PlayerInputManager>
{
    public Vector2 direction;
    public WeaponController m_GunFireController;
    public UI_Manager m_UIManager;
    public PlayerInput m_playerInput;
    private Rigidbody m_RigidBody;
    private CapsuleCollider m_Collider;
    private void Start()
    {
        m_Collider = GetComponentInChildren<CapsuleCollider>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        if (m_playerInput.actions["Sprint"].IsPressed() && m_UIManager.SprintAmount > 0)
        {
            if (direction.y <= 0)
                return;
            m_UIManager.SprintCircleDecrease();
            OnSprint();
            Controller.Instance.m_Speed = Mathf.Lerp(Controller.Instance.m_Speed, 1f, Time.deltaTime);
        }
        else
        {
            if (direction != Vector2.zero && !m_playerInput.actions["Crouch"].IsPressed())
                SetWalking();
            m_UIManager.SprintCircleIncrease();
            Controller.Instance.m_Speed = Mathf.Lerp(Controller.Instance.m_Speed, 0.3f, Time.deltaTime);
        }
    }
    public void OnCrouch(InputValue val)
    {
        if (m_playerInput.actions["Crouch"].IsPressed())
        {
            AnimationController.Instance.SetAnimation("Crouch", true);
            m_Collider.center = new Vector3(0, 0.7579631f, 0);
            m_Collider.height = 1.515926f;
            Controller.Instance.Movement = Controller.MovementState.Crouching;
        }
        else
        {
            AnimationController.Instance.SetAnimation("Crouch", false);
            m_Collider.center = new Vector3(0, 0.9406704f, 0);
            m_Collider.height = 1.881341f;
            if (direction == Vector2.zero)
                Controller.Instance.Movement = Controller.MovementState.Standing;
        }
    }
    public void OnFlashLight()
    {
        if (WeaponController.Instance.weapon == null)
            return;

        if (WeaponController.Instance.weapon.SpotLight.GetComponent<Light>().enabled == false)
            WeaponController.Instance.weapon.SpotLight.GetComponent<Light>().enabled = true;
        else
            WeaponController.Instance.weapon.SpotLight.GetComponent<Light>().enabled = false;
    }
    public void OnMove(InputValue val)
    {
        direction = val.Get<Vector2>();

        if (direction == Vector2.zero)
        {
            AnimationController.Instance.SetAnimation("Walking", false);
            if (m_playerInput.actions["Crouch"].IsPressed())
                Controller.Instance.Movement = Controller.MovementState.Crouching;
            else
                Controller.Instance.Movement = Controller.MovementState.Standing;
            return;
        }
        if (Controller.Instance.Movement == Controller.MovementState.Running || Controller.Instance.Movement == Controller.MovementState.Crouching)
            return;
        SetWalking();
    }
    public void SetWalking()
    {
        AnimationController.Instance.SetAnimation("Running", false);
        AnimationController.Instance.SetAnimation("Walking", true);
        Controller.Instance.Movement = Controller.MovementState.Moving;
    }
    public void SetRunning()
    {
        AnimationController.Instance.SetAnimation("Walking", false);
        AnimationController.Instance.SetAnimation("Running", true);
        Controller.Instance.Movement = Controller.MovementState.Running;
    }
    public void OnSprint()
    {
        if (direction == Vector2.zero)
        {
            AnimationController.Instance.SetAnimation("Running", false);
            Controller.Instance.Movement = Controller.MovementState.Standing;
            return;
        }
        if (direction.y <= 0)
            return;
        if (m_UIManager.SprintAmount == 0)
        {
            SetWalking();
            return;
        }
        if (m_playerInput.actions["Crouch"].IsPressed())
            Controller.Instance.RunSpeed = Controller.Instance.CrouchRunSpeed;
        else
            Controller.Instance.RunSpeed = Controller.Instance.DefaultRunSpeed;
        SetRunning();
    }
    public void OnReload()
    {
        if (WeaponController.Instance.Reload())
        {
            m_RigidBody.velocity = Vector3.zero;
            direction = Vector2.zero;
            SlotSystem.Instance.CurrentSlot.SetReload(true);
        }
    }
    public void OnSwap(InputValue val)
    {
        int key;
        if ((m_playerInput.actions["Swap"].IsPressed() && val.Get<Vector2>() != Vector2.zero) && WeaponController.Instance.WeaponEnum == WeaponController.WeaponSituation.Holding)
        {
            key = val.Get<Vector2>() == Vector2.up ? 1 : 2;
            SlotSystem.Instance.ChangeWeapon(key);
        }
    }

}
