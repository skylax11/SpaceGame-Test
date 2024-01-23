using Assets.Scripts.Weapons;
using Assets.Scripts.Weapons.Dual_Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class GunFireController : MonobehaviourSingleton<GunFireController>
{
    [Header("Inputs & Weapon")]
    private float _fireCounter;
    [SerializeField] Weapon Weapon;
    private PlayerInput m_playerInput;
    public WeaponSituation WeaponEnum;
    [Header("Bullet")]
    private Queue<GameObject> _bullets = new Queue<GameObject>();
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] public Transform bulletTransform;
    [SerializeField] Transform bulletHierarchy;
    [Header("UI")]
    [SerializeField] UI_Manager m_UIManager;
    [Header("Other")]
    [SerializeField] Riggings _holdingRiggings;
    [SerializeField] Riggings _aimingRiggings;
    [Header("Weapons")]
    [SerializeField] Weapon Weapon1;
    [SerializeField] Weapon Weapon2;

    public enum WeaponSituation
    {
        Firing,
        Holding,
        Reloading
    }

    void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_UIManager = GetComponent<UI_Manager>();
        m_UIManager.UpdateAmmo(Weapon.Ammo);
        m_UIManager.UpdateMagazine(Weapon.Magazine);
    }
    void Update()
    {
        if (m_playerInput.actions["shot"].IsPressed() && Weapon.Ammo > 0)
        {
            WeaponEnum = WeaponSituation.Firing;
            SetRigSettings.Instance.LetSetRigs = true;
            AnimationController.Instance.SetAnimation("IsShooting", true);
            if(SetRigSettings.Instance._aimingRig.weight > 0.85f)
            Fire();
            m_UIManager.UpdateAmmo(Weapon.Ammo);
        }
        else
        {
            WeaponEnum = WeaponSituation.Holding;
            SetRigSettings.Instance.LetSetRigs = false;
            AnimationController.Instance.SetAnimation("IsShooting", false);
        }
    }
    private void Fire()
    {
        if (Time.time > _fireCounter)
        {
            _fireCounter = Weapon.FireFreq + Time.time;
            if(!Weapon.isDualWeapon)
                Static_ObjectPooling.do_ObjectPooling(Weapon,bulletPrefab,bulletHierarchy,bulletTransform,_bullets);
            else
            {
                var dualWeapon = Weapon as DualWeapon; 
                Static_ObjectPooling.do_ObjectPooling(Weapon, bulletPrefab, bulletHierarchy, dualWeapon.BulletLeft, _bullets);
                Static_ObjectPooling.do_ObjectPooling(Weapon, bulletPrefab, bulletHierarchy, dualWeapon.BulletRight, _bullets);
            }
            Weapon.Fire();
        }
    }
    public bool Reload()
    {
        if (Weapon.Magazine > 0)
        {
            Weapon.Reload();
            m_UIManager.UpdateAmmo(Weapon.Ammo);
            m_UIManager.UpdateMagazine(Weapon.Magazine);
            return true;
        }
        return false;
    }
    static int sayi = 0;
    public void ChangeWeapon()   // Deðiþecek test amaçlý küçük bir silah deðiþimi yazýldý.
    {        
        sayi++;
        if (sayi % 2 == 1)
        {
            Weapon = Weapon2;
            Weapon1.gameObject.SetActive(false);
            Weapon2.gameObject.SetActive(true);
        }
        else
        {
            Weapon = Weapon1;
            Weapon2.gameObject.SetActive(false);
            Weapon1.gameObject.SetActive(true);
        }
            SetRigSettings.Instance.SetRigDatas(Weapon);

    }

}
