using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonobehaviourSingleton<AnimationController>
{
    public Animator animator;
    public GameObject Magazine;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void SetAnimation(string name, bool situation)
    {
        animator.SetBool(name, situation);
    }
    public void SetReloadForEditor()
    {
        SlotSystem.Instance.CurrentSlot.SetReload(false);
    }
    public void ReloadAnimationTakeOffMagazine(GameObject newMagazine)
    {
        var income = SlotSystem.Instance.CurrentSlot.ReloadAnimation(newMagazine,Magazine);
        StartCoroutine("SetCollider",income);
    }
    IEnumerator SetCollider(GameObject income)
    {
        yield return new WaitForSeconds(0.5f);
        income.transform.GetComponent<Collider>().enabled = true;
    }
    public void ReloadAnimationSetEnableMagazine()
    {
        Magazine.SetActive(true);
    }
}
