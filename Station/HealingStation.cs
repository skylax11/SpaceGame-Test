using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealingStation : MonoBehaviour, IStation
{
    private readonly string DESCRIPTION = "Hold E to heal up";
    public string Description
    {
        get
        {
            return DESCRIPTION;
        }
        set
        {

        }
    }
    public void Execute(Controller player)
    {
        
        Character character = player.m_Character;
        if (character.Health >= 100)
            return;
        int previousHealth = character.Health;
        character.Health += 1;
        character.UpdateUI(previousHealth);
        
    }
}
