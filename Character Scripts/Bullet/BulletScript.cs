using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BulletScript : MonoBehaviour
{
    [SerializeField] Rigidbody m_RigidBody;
    [SerializeField] GameObject effect;
    public int damage;
    private float speed = 60f;
    void Start()
    {
        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<StoreCollider>().ignored);
        ReCall();
    }


    public IEnumerator SetVisible()
    {
        yield return new WaitForSeconds(2f);
        ResetVelo();
        gameObject.SetActive(false);
    }
    public void ReCall()
    {
        m_RigidBody.velocity = GetSpeed();
        StartCoroutine("SetVisible");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.transform.TryGetComponent(out Controller player))
            gameObject.SetActive(false);
        if (collision.transform.TryGetComponent(out IWall wallType))
        {
            GameObject theEffect = Instantiate(wallType.EffectGameObjectPrefab);
            theEffect.transform.position = transform.position;

            if (wallType.Effect != null)
                wallType.Effect.Play();
        }
        if (collision.transform.TryGetComponent(out IHuman human))
            human.TakeDamage(damage, collision.transform.position - transform.position);
    }
    public void ResetVelo()
    {
        m_RigidBody.velocity = Vector3.zero;
        m_RigidBody.velocity = GetSpeed();
    }
    public Vector3 GetSpeed()
    {
        return transform.forward * speed;
    }

}
