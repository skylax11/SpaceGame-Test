using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonobehaviourSingleton<UI_Manager>
{
    [SerializeField] Image HealthBar;
    [SerializeField] Image SprintBar;
    [SerializeField] TextMeshProUGUI Ammo;
    [SerializeField] TextMeshProUGUI Magazine;
    [SerializeField] public TextMeshProUGUI Health;
    [SerializeField] TextMeshProUGUI Sprint;
    [SerializeField] TextMeshProUGUI InfoText;
    [SerializeField] public GameObject InfoPanel;
    [Range(0,1)]public float SprintAmount;
    [SerializeField] WeaponStands lastOne;
    private void Update()
    {
        Ray lookAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(lookAt, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.transform.TryGetComponent(out WeaponStands stand) != false && Vector3.Distance(this.transform.position,hitInfo.transform.position) < 5)
            {
                lastOne = stand;
                InfoText.text = $"Press 'E' to pickup {stand.weapon.WeaponSO.name}";
                stand.text.text = stand.weapon.WeaponSO.name;
                SetActivePanel(true);
                if (PlayerInputManager.Instance.m_playerInput.actions["PickUp"].IsPressed())
                    SlotSystem.Instance.PickUp(stand);
                return;
            }
            SetActivePanel(false);
        }
    }
    public void SetActivePanel(bool situation)
    {
        InfoPanel.SetActive(situation);
        if (lastOne != null)
        {
            lastOne.text.enabled = situation;
        }
    }
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
