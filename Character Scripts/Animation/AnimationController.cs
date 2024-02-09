using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEngine.InputSystem.HID.HID;

public class AnimationController : MonobehaviourSingleton<AnimationController>
{
    public Animator animator;
    public GameObject Magazine;
    private void Start() => animator = GetComponent<Animator>();
    public void SetAnimation(string name, bool situation) => animator.SetBool(name, situation);
    public void SetReloadForEditor() => SetAnimation("Reload", false);
    public void ReloadAnimationSetEnableMagazine() => Magazine.SetActive(true);
    public void ReloadAnimationTakeOffMagazine(GameObject newMagazine) => SlotSystem.Instance.CurrentSlot.ReloadAnimation(newMagazine, Magazine);
    public void SetJumpAnimation()
    {
        animator.applyRootMotion = false;
        SetAnimation("Jump", false);
    }
    public void ChangeMotion(int layer,string Name, AnimationClip clip)
    {
        var controller = animator.runtimeAnimatorController as AnimatorController;
        controller.layers[layer].stateMachine.states.FirstOrDefault(x => x.state.name == Name).state.motion = clip;
    }
}
    