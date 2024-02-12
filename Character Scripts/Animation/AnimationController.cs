using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationController : MonobehaviourSingleton<AnimationController>
{
    public Animator animator;
    public GameObject Magazine;
    private void Start() => animator = GetComponent<Animator>();
    public void SetAnimation(string name, bool situation) => animator.SetBool(name, situation);
    public void SetReloadForEditor() => SlotSystem.Instance.CurrentSlot.SetReload(false);
    public void ReloadAnimationSetEnableMagazine() => Magazine.SetActive(true);
    public void ReloadAnimationTakeOffMagazine(GameObject newMagazine) => SlotSystem.Instance.CurrentSlot.ReloadAnimation(newMagazine, Magazine);
    public void SetJumpAnimation()
    {
        animator.applyRootMotion = false;
        SetAnimation("Jump", false);
    }
}
    