using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicArea : MonoBehaviour
{
    private Character character;
    public int damage;
    private void TakeDamage()
    {
        if(character != null)
            character.TakeDamage(damage, Vector3.zero);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Character character))
        {
            this.character = character;
            InvokeRepeating("TakeDamage",0.1f,0.2f);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Character character))
            CancelInvoke("TakeDamage");
    }
}
