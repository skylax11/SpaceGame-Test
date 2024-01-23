using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour 
{
    [SerializeField] Image HealthBar;
    [SerializeField] Image SprintBar;
    [SerializeField] TextMeshProUGUI Ammo;
    [SerializeField] TextMeshProUGUI Magazine;
    [SerializeField] TextMeshProUGUI Health;
    [SerializeField] TextMeshProUGUI Sprint;

    [Range(0,1)]public float SprintAmount;
    public void SprintCircleIncrease()
    {
        SprintAmount = SprintBar.fillAmount += Time.deltaTime * 0.08f;
        Sprint.text = ((int)(SprintAmount*100)).ToString();
        SprintAmount = Mathf.Clamp(SprintAmount, 0, 1);
    }
    public void SprintCircleDecrease()
    {
        SprintAmount = SprintBar.fillAmount -= Time.deltaTime * 0.2f;
        Sprint.text = ((int)(SprintAmount * 100)).ToString();
        SprintAmount = Mathf.Clamp(SprintAmount, 0, 1);
    }
    public void UpdateAmmo(int ammo) => Ammo.text = ammo.ToString();
    public void UpdateMagazine(int mag) => Magazine.text = mag.ToString();

}
