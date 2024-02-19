using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCollider : MonoBehaviour
{
    [SerializeField] CapsuleCollider ignore;
    private void Start() => Physics.IgnoreCollision(GetComponent<Collider>(), ignore);
    private void Update() => transform.localPosition = Vector3.zero;
}
