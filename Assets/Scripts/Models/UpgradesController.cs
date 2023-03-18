public class UpgradesController
{
    private MoneyFlowResolver _moneyFlowResolver;

    public void Init(MoneyFlowResolver moneyFlowResolver)
    {
        _moneyFlowResolver = moneyFlowResolver;
    }

    public bool UpgradeBusiness(
        ref BusinessComponent businessComponent, BusinessMultiplierUpgradesComponent businessUpgrades, ViewObject<BusinessPanel> viewObject)
    {
        if (_moneyFlowResolver.TrySpendMoney(GetUpgradeCost(businessComponent.UpgradeLevel, businessComponent.BusinessData.DefaultCost)))
        {
            businessComponent.UpgradeLevel++;
            viewObject.View.UpdateIncomeValue(GetIncomeValueByBusinessUpgrade(businessComponent, businessUpgrades, businessComponent.BusinessData.DefaultIncome));
            viewObject.View.UpdateUpgradeLevel(businessComponent.UpgradeLevel);
            viewObject.View.UpdateBusinessUpgradeLevelCost(GetUpgradeCost(businessComponent.UpgradeLevel, businessComponent.BusinessData.DefaultCost));

            return true;
        }

        return false;
    }

    public void SetBusinessUpgradeLevel(
        ref BusinessComponent businessComponent, BusinessMultiplierUpgradesComponent businessUpgrades, ViewObject<BusinessPanel> viewObject,
        int levelToSet)
    {
        businessComponent.UpgradeLevel = levelToSet;
        viewObject.View.UpdateIncomeValue(GetIncomeValueByBusinessUpgrade(businessComponent, businessUpgrades, businessComponent.BusinessData.DefaultIncome));
        viewObject.View.UpdateUpgradeLevel(businessComponent.UpgradeLevel);
        viewObject.View.UpdateBusinessUpgradeLevelCost(GetUpgradeCost(businessComponent.UpgradeLevel, businessComponent.BusinessData.DefaultCost));
    }

    public void ActivateBusinessMultiplier(
        BusinessComponent businessComponent, ref BusinessMultiplierUpgradesComponent businessUpgrades, ViewObject<BusinessPanel> viewObject,
        BusinessUpgradeData identificator)
    {
        for (int i = 0; i < businessUpgrades.Value.Count; i++)
        {
            if (!businessUpgrades.ActivationMap[i] && businessUpgrades.Value[i].Data == identificator)
            {
                businessUpgrades.ActivationMap[i] = true;
                businessUpgrades.Value[i].View.MarkBought();

                viewObject.View.UpdateIncomeValue(GetIncomeValueByBusinessUpgrade(businessComponent, businessUpgrades, businessComponent.BusinessData.DefaultIncome));
            }
        }
    }
    
    public bool UpgradeBusinessMuiltiplier(
        BusinessComponent businessComponent, ref BusinessMultiplierUpgradesComponent businessUpgrades, ViewObject<BusinessPanel> viewObject,
        BusinessUpgradeData identificator)
    {
        for (int i = 0; i < businessUpgrades.Value.Count; i++)
        {
            if (!businessUpgrades.ActivationMap[i] && businessUpgrades.Value[i].Data == identificator)
            {
                if (_moneyFlowResolver.TrySpendMoney(identificator.Cost))
                {
                    businessUpgrades.ActivationMap[i] = true;
                    businessUpgrades.Value[i].View.MarkBought();

                    viewObject.View.UpdateIncomeValue(GetIncomeValueByBusinessUpgrade(businessComponent, businessUpgrades, businessComponent.BusinessData.DefaultIncome));

                    return true;
                }
            }
        }


        return false;
    }

    public float GetIncomeValueByBusinessUpgrade(BusinessComponent businessComponent, BusinessMultiplierUpgradesComponent businessUpgrades, float defaultIncome)
    {
        float increaseIncomeMultiplier = 0f;

        for (int i = 0; i < businessUpgrades.Value.Count; i++)
        {
            if (businessUpgrades.ActivationMap[i])
            {
                increaseIncomeMultiplier += 1f + (businessUpgrades.Value[i].Data.IncomeMultiplier / 100f);
            }
        }

        return businessComponent.UpgradeLevel * defaultIncome * (increaseIncomeMultiplier == 0f ? 1f : increaseIncomeMultiplier);
    }

    public float GetUpgradeCost(int businessLevel, float businessCost)
    {
        return (1 + businessLevel) * businessCost;
    }
}