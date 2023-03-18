using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class BusinessUpgradeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _upgradeCostTitle, _boughtTitle;
    [SerializeField] private TextMeshProUGUI _incomePercentValue, _upgradeCost;

    private BusinessUpgradeData _currentData;

    public void Init(BusinessUpgradeData data, Action<BusinessUpgradeData> upgradeCallback)
    {
        _currentData = data;

        _title.text = data.Title;
        _incomePercentValue.text = $"{data.IncomeMultiplier}%";
        _upgradeCost.text = $"{data.Cost}$";

        _button.onClick.AddListener(() => 
        {
            upgradeCallback?.Invoke(_currentData);
        });
    }

    public void MarkBought()
    {
        _upgradeCostTitle.gameObject.SetActive(false);
        _upgradeCost.gameObject.SetActive(false);
        _boughtTitle.gameObject.SetActive(true);
        _button.interactable = false;
    }

    public BusinessUpgradeData GetCurrentData()
    {
        return _currentData;
    }
}