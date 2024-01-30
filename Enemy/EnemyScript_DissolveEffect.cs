using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript_DissolveEffect : MonoBehaviour
{
    [Header("Dissolving")]
    [SerializeField] Material Dissolve;
    [SerializeField] float _dissolveSpeed;
    private float _dissolveTime = -1f;

    [SerializeField] SkinnedMeshRenderer[] m_SkinnedMeshMaterials;
    [SerializeField] MeshRenderer[] m_Materials;
    private Collider m_Collider;
    private void Start()
    {
        m_SkinnedMeshMaterials = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_Materials = GetComponentsInChildren<MeshRenderer>();
        m_Collider = GetComponent<Collider>();
    }
    public void DoDissolving()
    {
        Dissolve.SetFloat("_Timer", -1f);
        foreach (var m in m_SkinnedMeshMaterials)
        {
            Material[] materials = m.materials;

            for (int i = 0; i < materials.Length; i++)
                materials[i] = Dissolve;

            m.materials = materials;
        }
        foreach (var m in m_Materials)
        {
            Material[] materials = m.materials;

            for (int i = 0; i < materials.Length; i++)
                materials[i] = Dissolve;

            m.materials = materials;
        }
        InvokeRepeating("SetDissolving", 0.1f, 0.01f);
    }
    public void SetDissolving()
    {
        float dissolve = Dissolve.GetFloat("_Timer");
        _dissolveTime += _dissolveSpeed;
        foreach (var m in m_SkinnedMeshMaterials)
        {
            m.material.SetFloat("_Timer", _dissolveTime);
        }
        foreach (var m in m_Materials)
        {
            m.material.SetFloat("_Timer", _dissolveTime);
        }
        if (_dissolveTime >= 1)
        {
            CancelInvoke("SetDissolving");
            gameObject.SetActive(false);
        }
    }
}
