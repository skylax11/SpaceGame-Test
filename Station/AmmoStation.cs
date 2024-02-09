using Assets.Scripts.Character_Scripts.Inventory;
using Assets.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoStation : MonoBehaviour , IStation
{
    private readonly string DESCRIPTION = "Press E to get ammo";
    public string Description
    {
        get
        {
            return DESCRIPTION;
        }
        set
        {

        }
    }
    public void Execute(Controller player)
    {
        Weapon currentSlot = player.GetComponent<SlotSystem>().CurrentSlot;
        currentSlot.Ammo = currentSlot.FullAmmo;
        currentSlot.Magazine = 240;
        UI_Manager.Instance.UpdateAmmo(currentSlot.Ammo);
        UI_Manager.Instance.UpdateMagazine(currentSlot.Magazine);
    }
}
