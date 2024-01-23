using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBox : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Ground")
        {
            print("sa");
            StartCoroutine("SetProps");
        }
    }
    IEnumerator SetProps()
    {
        yield return new WaitForSeconds(0.2f);
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
    }
}
