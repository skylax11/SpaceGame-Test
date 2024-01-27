using Assets.Scripts.Wall;
using Assets.Scripts.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

namespace Assets.Scripts.Character_Scripts.Inventory
{
    public class SlotSystem : MonobehaviourSingleton<SlotSystem>
    {
        public GameObject Inventory;
        public Weapon Slot1;
        public Weapon Slot2;
        public Weapon CurrentSlot;
        private UI_Manager m_UIManager;
        [SerializeField] Weapon _emptySlot; // no need to code thousands of null checks,, it simply solves the problem
        private void Start()
        {
            m_UIManager = GetComponent<UI_Manager>();
            CurrentSlot = Slot1;
        }
        public void ChangeWeapon(int key) 
        {
            if (key == 1)
            {
                Slot1.gameObject.SetActive(true);
                Slot2.gameObject.SetActive(false);
                Slot1.transform.name = Slot1.WeaponSO.Name;
                Slot2.transform.name = "Disabled" + Slot2.WeaponSO.Name;
                CurrentSlot = Slot1;
            }
            else                         // WEAPON NAMES ARE REDECLARED CAUSE PREVENT ANY BUGS WHICH CAN OCCUR FROM ANIMATION
            {                            
                Slot2.gameObject.SetActive(true);
                Slot1.gameObject.SetActive(false);
                Slot2.transform.name = Slot2.WeaponSO.Name;
                Slot1.transform.name = "Disabled" + Slot1.WeaponSO.Name;
                CurrentSlot = Slot2;
            }
            if (CurrentSlot != _emptySlot)
                SetRigSettings.Instance.SetRigDatas(CurrentSlot);
            else
                SetRigSettings.Instance.ResetRigDatas();
            AnimationController.Instance.Magazine = CurrentSlot.BulletBox;
            UpdateAmmoInfo();
        }
        public void PickUp(WeaponStands stand)
        {
            if (Slot1 != _emptySlot && Slot2 != _emptySlot)
                CurrentSlot.ThrowWeaponAway();   // i love polymorphism...

            Weapon pickUpWeapon = stand.weapon;
            Transform boxPos = stand.weapon.BulletBox.transform;

            PickUp_TransformSettings(stand,pickUpWeapon);
            PickUp_WeaponControllerSettings();

            stand.text.enabled = false;
            Destroy(stand); // Removing script from object. It's no longer a Weapon stander.

            if (Slot1 == _emptySlot)
                Slot1 = pickUpWeapon;
            else if (Slot2 == _emptySlot)
                Slot2 = pickUpWeapon;

            CurrentSlot = pickUpWeapon;

            SetRigSettings.Instance.SetRigDatas(CurrentSlot);
            UpdateAmmoInfo();
        }
        public void UpdateAmmoInfo()
        {
            m_UIManager.UpdateAmmo(CurrentSlot.Ammo);
            m_UIManager.UpdateMagazine(CurrentSlot.Magazine);
        }
        public void PickUp_TransformSettings(WeaponStands stand,Weapon pickUpWeapon)
        {
            Transform boxPos = stand.weapon.BulletBox.transform;
            pickUpWeapon.gameObject.transform.parent = Inventory.transform;
            pickUpWeapon.transform.localPosition = pickUpWeapon.WeaponSO.WeaponPose;
            pickUpWeapon.transform.localRotation = Quaternion.Euler(pickUpWeapon.WeaponSO.WeaponRot.x, pickUpWeapon.WeaponSO.WeaponRot.y, pickUpWeapon.WeaponSO.WeaponRot.z);
            pickUpWeapon.transform.localScale = pickUpWeapon.WeaponSO.WeaponSize;
            stand.weapon.BulletBox.transform.localPosition = boxPos.localPosition;
            stand.weapon.BulletBox.transform.localRotation = boxPos.localRotation;
            AnimationController.Instance.Magazine = stand.weapon.BulletBox;
        }
        public void PickUp_WeaponControllerSettings() => WeaponController.Instance.bulletTransform = CurrentSlot.BulletPos;

    }
}
