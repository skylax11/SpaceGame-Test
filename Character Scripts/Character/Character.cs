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
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        UpdateUI(damage);
        
    }
    public void UpdateUI(int damage)
    {
        int oldHealth = _health;
        _health -= damage;
        UI_Manager.Instance.Health.text = ((int)Mathf.Lerp(oldHealth, _health,Time.deltaTime*15f)).ToString();
    }
}
