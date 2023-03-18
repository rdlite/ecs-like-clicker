using UnityEngine;

public class MoneyFlowResolver
{
    private UIRoot _uiRoot;
    private float _currentMoneyAmount;

    public void Init(UIRoot uiRoot)
    {
        _uiRoot = uiRoot;
    }

    public void SetMoneyAmount(float value)
    {
        _currentMoneyAmount = value;
        UpdateCurrentMoneyView();
    }

    public void AddMoney(float amount)
    {
        _currentMoneyAmount += amount;
        UpdateCurrentMoneyView();
    }

    public bool TrySpendMoney(float moneyToSpend)
    {
        if (_currentMoneyAmount >= moneyToSpend)
        {
            _currentMoneyAmount -= moneyToSpend;
            UpdateCurrentMoneyView();
            return true;
        }

        return false;
    }

    private void UpdateCurrentMoneyView()
    {
        _uiRoot.GetPanel<UpgradesPanel>().SetCurrentMoneyAmount(_currentMoneyAmount);
    }

    public float GetMoneyAmount()
    {
        return _currentMoneyAmount;
    }
}