using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager_Player : MonoBehaviour
{
    [Header("Player's Scripts")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Controller playerController;
    [SerializeField] RigBuilder playerRigBuilder;

    [Header("Volume Settings")]
    [SerializeField] Volume volume;
    private Vignette vignet;
    private float vignetIntensity  = 1f;
    private float vignetSmoothness = 1f;

    private void Start()
    {
        playerInput.enabled = playerController.enabled = playerRigBuilder.enabled = false;
        volume.profile.TryGet(out vignet);
        StartCoroutine("Enable");
        InvokeRepeating("SetVolume", 0.1f, 0.04f);
    }
    private void SetVolume()
    {
        vignetIntensity  -= Time.deltaTime * 0.1f;
        vignetSmoothness -= Time.deltaTime * 0.08f;
      
        vignetIntensity  = Mathf.Clamp(vignetIntensity, 0.5f, 1f);
        vignetSmoothness = Mathf.Clamp(vignetSmoothness, 0.7f, 1f);

        vignet.intensity.Override(vignetIntensity);
        vignet.smoothness.Override(vignetSmoothness);

        if (vignetIntensity == 0.5f && vignetSmoothness == 0.7f)
            CancelInvoke("SetVolume");
    }
    IEnumerator Enable()
    {
        yield return new WaitForSeconds(8.8f);
        playerInput.enabled = playerController.enabled = playerRigBuilder.enabled = true;
    }
}
