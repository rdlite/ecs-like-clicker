using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessPanel : MonoBehaviour
{
    public event Action<BusinessUpgradeData> OnUpgradeBusinessButtonPressed;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _upgradeLevel, _incomeValue;
    [SerializeField] private Button _upgradeBusinessLevelButton;
    [SerializeField] private TextMeshProUGUI _upgradeBusinessLevelCost;
    [SerializeField] private Transform _upgradeButtonsContainer;

    private List<BusinessUpgradeButton> _createdUpgradeButtons;
    private Configs _configs;
    private BusinessData _currentBusinessData;
    private float _deactivatedAlpha;

    public void Init(
        BusinessData data, Configs configs, Action<BusinessData> businessUpgradeButtonPressCallback)
    {
        _configs = configs;

        _title.text = data.Title;

        SetSliderProgression(0f);
        UpdateUpgradeLevel(0);
        UpdateBusinessUpgradeLevelCost(data.DefaultCost);
        UpdateIncomeValue(data.DefaultIncome);

        _currentBusinessData = data;
        _deactivatedAlpha = configs.DeactivatedBusinessPanelAlpha;

        _upgradeBusinessLevelButton.onClick.AddListener(() => 
        {
            businessUpgradeButtonPressCallback?.Invoke(data);
        });

        CreateUpgradeButtons();
    }

    public void UpdateBusinessUpgradeLevelCost(float newValue)
    {
        _upgradeBusinessLevelCost.text = ((int)newValue).ToString();
    }

    public void UpdateUpgradeLevel(int newValue)
    {
        _upgradeLevel.text = newValue.ToString();
    }

    public void UpdateIncomeValue(float newValue)
    {
        _incomeValue.text = $"{(int)newValue}$";
    }

    public void SetEnable(bool value)
    {
        _canvasGroup.alpha = value ? 1f : _deactivatedAlpha;
        _canvasGroup.interactable = value;
    }

    public void SetSliderProgression(float value)
    {
        _progressSlider.value = value;
    }

    private void CreateUpgradeButtons()
    {
        _createdUpgradeButtons = new List<BusinessUpgradeButton>();
        foreach (var item in _currentBusinessData.UpgradeButtons)
        {
            BusinessUpgradeButton newBusinessUpgradeButton = Instantiate(_configs.PrefabsContainer.BusinessUpgradeButton);
            newBusinessUpgradeButton.transform.SetParent(_upgradeButtonsContainer);
            newBusinessUpgradeButton.transform.localScale = Vector3.one;
            newBusinessUpgradeButton.transform.localRotation = Quaternion.identity;
            newBusinessUpgradeButton.Init(item, OnUpgradeBusinessButtonPressed);
            _createdUpgradeButtons.Add(newBusinessUpgradeButton);
        }
    }

    public List<BusinessUpgradeButton> GetUpgradeButtonsView()
    {
        return _createdUpgradeButtons;
    }
}