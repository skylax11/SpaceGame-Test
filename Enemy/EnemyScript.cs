using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour , IHuman
{
    [Header("Dissolving")]
    [SerializeField] Material Dissolve;
    [SerializeField] float _dissolveSpeed;
    [SerializeField] int _health;
    private float _dissolveTime = -1f;

    private MeshRenderer m_Material;
    private Collider m_Collider;
    private void Start()
    {
        m_Material = GetComponent<MeshRenderer>();
        m_Collider = GetComponent<Collider>();
    }
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

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Dissolve.SetFloat("_Timer", -1f);
            m_Material.material = Dissolve;
            InvokeRepeating("SetDissolving",0.1f,0.01f);
            m_Collider.enabled = false;
            print(Dissolve.GetFloat("_Timer"));
        }
    }
    public void SetDissolving()
    {
        
        float dissolve = Dissolve.GetFloat("_Timer");
        _dissolveTime += _dissolveSpeed;
        m_Material.material.SetFloat("_Timer", _dissolveTime);
        if (dissolve >= 1)
        {
            CancelInvoke("SetDissolving");
            gameObject.SetActive(false);
        }
    }
}
