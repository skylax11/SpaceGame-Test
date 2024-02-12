using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DissappearEffect : MonoBehaviour
{
    [Header("Dissappear Effect")]
    [SerializeField] Material _dissappearMat;
    [SerializeField] float _dissappearSpeed;
    public float _dissappearTime = -1f;

    [Header("Skinned Mesh Renderer & Mesh Renderer")]
    [SerializeField] SkinnedMeshRenderer[] m_SkinnedMeshMaterials;
    [SerializeField] MeshRenderer[] m_Materials;

    [Header("Materials")]
    [SerializeField] Material[] _oldMaterialsChest;
    [SerializeField] List<Material[]> _oldMaterialsOther = new List<Material[]>();
    [SerializeField] Transform WeaponPivot;

    [Header("Dissappear Effect")]
    private bool isPressingTemp;
    public bool isVisible = true;
    [SerializeField] float _teleportSpeed;

    [Header("Sound Manager")]
    [SerializeField] SoundController m_SoundController;
    [Header("PlayerInputManager Script")]
    [SerializeField] PlayerInput m_playerInputManager;
    // Teleport
    [Header("")]
    private Material teleport;
    private bool reverseIt = false;
    private bool CaroutineForOnce = true;
    private float _teleportTime = -1f;
    

    private void Start() => SetMaterials();
    public void SetMaterials()
    {
        m_SkinnedMeshMaterials = GetComponentsInChildren<SkinnedMeshRenderer>();
        m_Materials = WeaponPivot.GetComponentsInChildren<MeshRenderer>();
        _oldMaterialsOther.Clear();
        foreach (var mesh in m_Materials)
            _oldMaterialsOther.Add(mesh.materials);

    }
    public void DoDissappearing(bool isPressing)
    {
        if (isPressingTemp != isPressing)
        {
            if(isPressing)          // SET MATERIALS ONLY BEFORE DISSAPPEARING.
                SetMaterials();

            _dissappearMat.SetFloat("_Timer", -1f);
            foreach (var m in m_SkinnedMeshMaterials)
            {
                Material[] materials = m.materials;

                for (int i = 0; i < materials.Length; i++)
                    materials[i] = _dissappearMat;

                m.materials = materials;
            }
            foreach (var m in m_Materials)
            {
                Material[] materials = m.materials;

                for (int i = 0; i < materials.Length; i++)
                    materials[i] = _dissappearMat;

                m.materials = materials;
            }
            isPressingTemp = isPressing;
        }
        SetDissappearing(isPressing);
    }
    public void SetDissappearing(bool isPressing)
    {        
        _dissappearTime += isPressing ?  _dissappearSpeed : -_dissappearSpeed;
        _dissappearTime = Mathf.Clamp(_dissappearTime, -1f, 1f);
        foreach (var m in m_SkinnedMeshMaterials)
        {
            Material[] materials = m.materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat("_Timer", _dissappearTime);
            m.materials = materials;
        }
        foreach (var m in m_Materials)
        {
            Material[] materials = m.materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat("_Timer", _dissappearTime);
            m.materials = materials;
        }

        if (_dissappearTime == -1)
        {
            m_SkinnedMeshMaterials[0].materials = _oldMaterialsChest;
            for (int i = 0; i < m_Materials.Length; i++)
                m_Materials[i].materials = _oldMaterialsOther[i];
        }

        m_SoundController.m_AudioSource_Walk.enabled = isVisible = _dissappearTime == 1 ? false : true;
        // if _dissappear time equals to 1 which means effect completed and player is invisible, so set isVisible and walk sounds to false either.
    }
    public void DoDissappearing(Material teleport)
    {
        m_playerInputManager.enabled = false;
        this.teleport = teleport;

        SetMaterials();

        teleport.SetFloat("_Timer", -1f);
        foreach (var m in m_SkinnedMeshMaterials)
        {
            Material[] materials = m.materials;

            for (int i = 0; i < materials.Length; i++)
                materials[i] = teleport;

            m.materials = materials;
        }
        foreach (var m in m_Materials)
        {
            Material[] materials = m.materials;

            for (int i = 0; i < materials.Length; i++)
                materials[i] = teleport;

            m.materials = materials;
        }
        CaroutineForOnce = true;
        AnimationController.Instance.SetAnimation("EnterTeleport", true);
        InvokeRepeating("SetDissappearing",0.1f,0.008f);
    }
    
    public void SetDissappearing()
    {
        _teleportTime += !reverseIt ? _teleportSpeed : -_teleportSpeed;
        _teleportTime = Mathf.Clamp(_teleportTime, -1f, 1f);

        foreach (var m in m_SkinnedMeshMaterials)
        {
            Material[] materials = m.materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat("_Timer", _teleportTime);
            m.materials = materials;
        }
        foreach (var m in m_Materials)
        {
            Material[] materials = m.materials;
            for (int i = 0; i < materials.Length; i++)
                materials[i].SetFloat("_Timer", _teleportTime);
            m.materials = materials;
        }
        if (_teleportTime == -1)
        {
            AnimationController.Instance.SetAnimation("ExitTeleport", false);
            m_SkinnedMeshMaterials[0].materials = _oldMaterialsChest;
            for (int i = 0; i < m_Materials.Length; i++)
                m_Materials[i].materials = _oldMaterialsOther[i];
            reverseIt = false;
            m_playerInputManager.enabled = true;
            CancelInvoke("SetDissappearing");
        }

        if (_teleportTime == 1 && CaroutineForOnce)
        {
            StartCoroutine("WaitProcess");
            CaroutineForOnce = false;
        }
        isVisible = _teleportTime == 1 ? false : true;
    }
    IEnumerator WaitProcess()
    {
        yield return new WaitForSeconds(1f);
        AnimationController.Instance.SetAnimation("ExitTeleport", true);
        AnimationController.Instance.SetAnimation("EnterTeleport", false);
        reverseIt = true;
    }
}
