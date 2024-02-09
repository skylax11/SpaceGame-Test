using Assets.Scripts.Character_Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_Manager : MonobehaviourSingleton<UI_Manager>
{
    [Header("Images")]
    public Image HealthBar;
    public Image SkillBar;
    [SerializeField] Image SprintBar;

    [Header("Texts & TMP")]
    [SerializeField] TextMeshProUGUI Ammo;
    [SerializeField] TextMeshProUGUI Magazine;
    [SerializeField] TextMeshProUGUI Sprint;
    [SerializeField] TextMeshProUGUI SkillText;
    [SerializeField] TextMeshProUGUI InfoText;
    public TextMeshProUGUI Health;

    [Header("Other")]
    public GameObject InfoPanel;
    [Range(0,1)] public float SprintAmount;
    [Range(0,1)] public float SkillAmount;

    [Header("Scripts")]
    [SerializeField] WeaponStands lastOne;
    private Controller m_Controller;

    [Header("Decoder")]
    [SerializeField] TMP_InputField password;
    [SerializeField] TextMeshProUGUI message;
    [SerializeField] GameObject PasswordPanel;
    private void Start() => m_Controller = GetComponent<Controller>();
    public void UpdateAmmo(int ammo) => Ammo.text = ammo.ToString();
    public void UpdateMagazine(int mag) => Magazine.text = mag.ToString();
    private void Update()
    {
        Ray lookAt = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(lookAt, out hitInfo, Mathf.Infinity) && Vector3.Distance(this.transform.position, hitInfo.transform.position) < 5)
        {
            if (hitInfo.transform.TryGetComponent(out WeaponStands stand) != false)
            {
                lastOne = stand;
                InfoText.text = $"Press 'E' to pickup {stand.weapon.WeaponSO.name}";
                stand.text.text = stand.weapon.WeaponSO.name;
                SetActivePanel(true);
                if (PlayerInputManager.Instance.m_playerInput.actions["PickUp"].IsPressed())
                    SlotSystem.Instance.PickUp(stand);
                return;
            }
            if (hitInfo.transform.TryGetComponent(out IStation station))
            {
                InfoText.text = station.Description;
                SetActivePanel(true);
                if (PlayerInputManager.Instance.m_playerInput.actions["PickUp"].IsPressed())
                    station.Execute(m_Controller);
                return;
            }
            if (hitInfo.transform.TryGetComponent(out KeyPanel key))
            {
                InfoText.text = key.Description;
                SetActivePanel(true);
                if (PlayerInputManager.Instance.m_playerInput.actions["PickUp"].IsPressed() && !key.isAnimated)
                {
                    SetPasswordPanel(true);
                    _currentKeyPanel = key;
                }
                return;
            }
            SetActivePanel(false);
        }
    }
    private KeyPanel _currentKeyPanel;
    public void SetPasswordPanel(bool situation) => PasswordPanel.SetActive(situation);
    public void CheckForPassword(string password)
    {
        if (_currentKeyPanel.CheckForPassword(password))
            SetPasswordPanel(false);
        else
            message.text = "Invalid Password";
    }
    public void SetActivePanel(bool situation)
    {
        InfoPanel.SetActive(situation);
        if (lastOne != null)
            lastOne.text.enabled = situation;
    }
    public void SprintCircleIncrease()
    {
        SprintAmount = SprintBar.fillAmount += Time.deltaTime * 0.08f;
        Sprint.text = ((int)(SprintAmount*100)).ToString();
        SprintAmount = Mathf.Clamp(SprintAmount, 0, 1);
    }
    public void SkillCircleIncrease()
    {
        SkillAmount = SkillBar.fillAmount += Time.deltaTime * 0.08f;
        SkillText.text = ((int)(SkillAmount * 100)).ToString();
        SkillAmount = Mathf.Clamp(SkillAmount, 0, 1);
    }
    public void SkillCircleDecrease()
    {
        SkillAmount = SkillBar.fillAmount -= Time.deltaTime * 0.4f;
        SkillText.text = ((int)(SkillAmount * 100)).ToString();
        SkillAmount = Mathf.Clamp(SkillAmount, 0, 1);
    }
    public void SprintCircleDecrease()
    {
        SprintAmount = SprintBar.fillAmount -= Time.deltaTime * 0.2f;
        Sprint.text = ((int)(SprintAmount * 100)).ToString();
        SprintAmount = Mathf.Clamp(SprintAmount, 0, 1);
    }
}
