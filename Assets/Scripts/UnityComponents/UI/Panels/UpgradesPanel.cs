using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradesPanel : UIPanel
{
    public event Action<BusinessData> OnBusinessUpgradeButtonPressed;

    [SerializeField] private Transform _businessesLayout;
    [SerializeField] private TextMeshProUGUI _currentMoneyText;

    private Dictionary<BusinessData, BusinessPanel> _businessPanelsMap;
    private float _targetMoneyAmount = 0f, _currentMoneyAmount = 0f;
    private float _moneyChangeSpeed = 8f;

    protected override void LocalInit()
    {
        _businessPanelsMap = new Dictionary<BusinessData, BusinessPanel>();
        _currentMoneyText.text = $"{0}$";
    }

    private void Update()
    {
        // плавное изменение количества денег (необязательная фича, но выглядит прикольно)
        if (Mathf.Abs(_currentMoneyAmount - _targetMoneyAmount) > .5f)
        {
            _currentMoneyAmount = Mathf.Lerp(_currentMoneyAmount, _targetMoneyAmount, _moneyChangeSpeed * Time.deltaTime);
            _currentMoneyText.text = $"{(int)_currentMoneyAmount}$";
        }
        else if (_currentMoneyAmount != _targetMoneyAmount)
        {
            _currentMoneyAmount = _targetMoneyAmount;
            _currentMoneyText.text = $"{(int)_currentMoneyAmount}$";
        }
    }

    public BusinessPanel CreateBusinessPanel(BusinessData businessData, Action<BusinessUpgradeData> upgradeButtonCallback)
    {
        BusinessPanel businessPanel = Instantiate(_configs.PrefabsContainer.BusinessPanelPrefab);
        businessPanel.transform.SetParent(_businessesLayout);
        businessPanel.transform.localScale = Vector3.one;
        businessPanel.transform.localRotation = Quaternion.identity;

        businessPanel.OnUpgradeBusinessButtonPressed += upgradeButtonCallback;

        businessPanel.Init(
            businessData, _configs, OnBusinessUpgradeButtonPressed);

        _businessPanelsMap.Add(businessData, businessPanel);

        return businessPanel;
    }

    public void SetCurrentMoneyAmount(float amount)
    {
        _targetMoneyAmount = amount;
    }

    public void SetBusinessPanelActive(BusinessData business, bool value)
    {
        _businessPanelsMap[business].SetEnable(value);
    }

    public List<BusinessPanel> GetBusinessPanels()
    {
        List<BusinessPanel> businessesToReturn = new List<BusinessPanel>();
        foreach (var item in _businessPanelsMap)
        {
            businessesToReturn.Add(item.Value);
        }
        return businessesToReturn;
    }
}