using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Player;
    void FixedUpdate() => transform.position = Vector3.Slerp(transform.position, Player.transform.position, Time.deltaTime * 2f);

}
