using Assets.Scripts.Character_Scripts.Inventory;
using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Dual_Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class WeaponController : MonobehaviourSingleton<WeaponController>
{
    [Header("Inputs & Weapon")]
    public Weapon weapon;
    public WeaponSituation WeaponEnum;
    public bool isAvailable = true;
    private float _fireCounter;
    private PlayerInput m_playerInput;
    

    [Header("Bullet")]
    public Transform bulletTransform;
    private Queue<GameObject> _bullets = new Queue<GameObject>();
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletHierarchy;


    [Header("UI")]
    [SerializeField] UI_Manager m_UIManager;

    [Header("Dissappear Effect")]
    [SerializeField] DissappearEffect m_PlayerDissappearEffect;
    public enum WeaponSituation
    {
        Firing,
        Holding,
        Reloading
    }

    void Start()
    {
        weapon = SlotSystem.Instance.Slot1;
        m_playerInput = GetComponent<PlayerInput>();
        m_UIManager = GetComponent<UI_Manager>();
        m_PlayerDissappearEffect = GetComponent<DissappearEffect>();
        if (weapon != null)
        {
            m_UIManager.UpdateAmmo(weapon.Ammo);
            m_UIManager.UpdateMagazine(weapon.Magazine);
        }
    }
    void Update()
    {
        if (weapon == null || m_PlayerDissappearEffect.isVisible == false)
            return;
        if ((m_playerInput.actions["shot"].IsPressed() && weapon.Ammo > 0)  && weapon.isReadyToUse)
        {
            WeaponEnum = WeaponSituation.Firing;
            SetRigSettings.Instance.LetSetRigs = true;
            AnimationController.Instance.SetAnimation("IsShooting", true);
            if(SetRigSettings.Instance._aimingRig.weight > 0.85f)
            Fire();
            m_UIManager.UpdateAmmo(weapon.Ammo);
        }
        else
        {
            if(SetRigSettings.Instance._aimingRig.weight == 0f)
            WeaponEnum = WeaponSituation.Holding;
            SetRigSettings.Instance.LetSetRigs = false;
            AnimationController.Instance.SetAnimation("IsShooting", false);
        }
    }
    private void Fire()
    {
        if (Time.time > _fireCounter)
        {
            _fireCounter = weapon.FireFreq + Time.time;
            SoundController.Instance.PlayWeaponSound(0.5f,1f,weapon.WeaponSO.WeaponShot);
            weapon.Fire(weapon,bulletPrefab,bulletHierarchy,_bullets);
        }
    }
    public bool Reload()
    {
        if (weapon != null && WeaponEnum == WeaponSituation.Holding)
        {
            if (weapon.Magazine > 0)
            {
                SoundController.Instance.PlayWeaponSound(0.5f, 1f, weapon.WeaponSO.WeaponReload);
                weapon.isReadyToUse = false;
                weapon.Reload();
                m_UIManager.UpdateAmmo(weapon.Ammo);
                m_UIManager.UpdateMagazine(weapon.Magazine);
                return true;
            }
        }
        return false;
    }
    public void SetAvailableSituation() => weapon.isReadyToUse = true;

}
