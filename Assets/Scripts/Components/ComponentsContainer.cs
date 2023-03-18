using System.Collections.Generic;

public struct BusinessComponent
{
    public int BusinessID;
    public BusinessData BusinessData;
    public int UpgradeLevel;
    public float CurrentIncomeTimer;
}

public struct BusinessMultiplierUpgradesComponent
{
    public List<bool> ActivationMap;
    public List<BusinessUpgradeDataItem> Value;
}
public struct BusinessUpgradeDataItem
{
    public BusinessUpgradeData Data;
    public BusinessUpgradeButton View;

    public BusinessUpgradeDataItem(BusinessUpgradeData data, BusinessUpgradeButton view)
    {
        Data = data;
        View = view;
    }
}

public struct ViewObject<T>
{
    public T View;
}

public struct UpgradeBusinessButtonTrigger 
{
    public BusinessUpgradeData Identificator;
}

public struct LoadLevelTrigger
{
    public int LevelID;
}

public struct SetUpgradeLevelTrigger
{
    public int UpgradeID;
}

public struct SetUpgradeMultiplierTrigger
{
    public List<BusinessUpgradeDataItem> UpgradeItems;
}

public struct UpgradeBusinessLevelTrigger { }

public struct IncomeEventTrigger { }

public struct ClosedBuissinessTagComponent { }