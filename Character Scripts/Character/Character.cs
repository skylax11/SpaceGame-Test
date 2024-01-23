using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour , IHuman
{
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

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }
}
