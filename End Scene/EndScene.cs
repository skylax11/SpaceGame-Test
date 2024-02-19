using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    [SerializeField] GameObject EndPanel;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            EndPanel.SetActive(true);
            StartCoroutine("Quit");
        }
    }
    IEnumerator Quit()
    {
        yield return new WaitForSeconds(3f);
        Application.Quit();
    }
}
