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
        animator.SetBool("Reload", false);
    }
    public void ReloadAnimationTakeOffMagazine(GameObject newMagazine)
    {
        Magazine.SetActive(false);
        GameObject income = Instantiate(newMagazine, null);
        income.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        income.transform.parent = null;
        income.transform.position = Magazine.transform.position;
        income.transform.rotation = Magazine.transform.rotation;
        income.transform.GetComponent<Rigidbody>().useGravity = true;
        income.transform.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * 40f);
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
