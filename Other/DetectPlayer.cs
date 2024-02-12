using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class DetectPlayer : MonoBehaviour
{
    [Header("Animation Camera")]
    [SerializeField] GameObject cam;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            cam.SetActive(true);
            StartCoroutine("DisableCam");
        }
    }
    IEnumerator DisableCam()
    {
        yield return new WaitForSeconds(1.6f);
        cam.SetActive(false);
    }
}
