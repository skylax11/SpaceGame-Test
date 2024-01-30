using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsOfEnemy : MonoBehaviour , IHuman
{
    private EnemyScript HeaderScript;
    [SerializeField] int _health;
    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }
    void Start() => HeaderScript = GetComponentInParent<EnemyScript>();
    public void TakeDamage(int damage, Vector3 hitDirection) => HeaderScript.TakeDamage(damage,hitDirection);
}
