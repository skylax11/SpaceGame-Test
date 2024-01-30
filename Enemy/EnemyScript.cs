using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour , IHuman
{
    [SerializeField] EnemyScript_DissolveEffect m_dissolveEffect;
    [SerializeField] EnemyScript_Rigs m_Rigs;
    [SerializeField] int _health;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    public void TakeDamage(int damage,Vector3 hitDirection)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Death(hitDirection);
            m_dissolveEffect.DoDissolving();
        }
    }
    public void Death(Vector3 hitDirection)
    {
        GetComponent<Collider>().enabled = false;
        foreach (var Rigidbody_Parts in GetComponentsInChildren<Rigidbody>())
        {
            Rigidbody_Parts.isKinematic = false;
            Rigidbody_Parts.useGravity  = true;
        }
        foreach (var Collider in GetComponentsInChildren<Collider>())
            Collider.enabled = true;
        var direction = hitDirection - transform.position;
        StartCoroutine("DisableAnimator");
        GetComponent<Rigidbody>().AddForce(direction.normalized * 30f * Time.deltaTime, ForceMode.Impulse);
    }
    IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponentInParent<Animator>().enabled = false;
    }
}
