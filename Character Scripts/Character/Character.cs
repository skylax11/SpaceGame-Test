using Assets.Scripts;
using Assets.Scripts.Scriptable_Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour , IHuman
{
    [SerializeField] int _health;
    [SerializeField] DissolveEffect m_DissolveEffect;
    [SerializeField] Transform _hierarchy;
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
    public void TakeDamage(int damage, Vector3 hitDirection) => UpdateUI(damage, hitDirection);

    public void UpdateUI(int damage , Vector3 hitDirection)
    {
        int oldHealth = _health;
        _health -= damage;
        UI_Manager.Instance.Health.text = ((int)Mathf.Lerp(oldHealth, _health,Time.deltaTime*15f)).ToString();
        UI_Manager.Instance.HealthBar.fillAmount = (Mathf.Lerp(UI_Manager.Instance.HealthBar.fillAmount, _health / 100, Time.deltaTime * 15f));
        if (_health <= 0)
        {
            Death(hitDirection);
            m_DissolveEffect.DoDissolving();
        }
    }
    private Rigidbody spine;
    public void Death(Vector3 hitDirection)
    {
        foreach (var Rigidbody_Parts in _hierarchy.GetComponentsInChildren<Rigidbody>())
        {
            Rigidbody_Parts.isKinematic = false;
            Rigidbody_Parts.useGravity = true;
            if (Rigidbody_Parts.transform.CompareTag("Spine"))
                spine = Rigidbody_Parts;
        }
        foreach (var Collider in _hierarchy.GetComponentsInChildren<Collider>())
            Collider.enabled = true;
        GetComponent<Collider>().enabled = false;
        StartCoroutine("DisableAnimator");
        spine.AddForce(new Vector3(0f, 0f, -hitDirection.normalized.z) * 5f, ForceMode.Impulse);

        this.enabled = false; // disabling script
    }
    IEnumerator DisableAnimator()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponentInParent<Animator>().enabled = false;
    }
}
