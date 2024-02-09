using Assets.Scripts;
using Assets.Scripts.Scriptable_Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour , IHuman
{
    [Header("Effects")]
    [SerializeField] DissolveEffect m_DissolveEffect;
    [SerializeField] DissappearEffect m_DissappearEffect;

    [Header("IHuman Props")]
    [SerializeField] int _health;

    [Header("Other")]
    [SerializeField] Transform _hierarchy;
    [SerializeField] PlayerInputManager m_PlayerInputManager;
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
    public void UpdateUI(int previousHealth)
    {
        UI_Manager.Instance.Health.text =  _health.ToString();
        UI_Manager.Instance.HealthBar.fillAmount = _health / 100f;
    }
    public void TakeDamage(int damage, Vector3 hitDirection) => IsDead(damage, hitDirection);

    public void IsDead(int damage , Vector3 hitDirection)
    {
        int previousHealth = _health;
        _health -= damage;
        UpdateUI(previousHealth);
        if (_health <= 0)
        {
            m_PlayerInputManager.enabled = false;
            Destroy(m_DissappearEffect); // Destroying Dissappear effect for preventing any possible bugs
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
