using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class InitUISystem : IEcsInitSystem
{
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, ViewObject<BusinessPanel>> _businessFilter;
    private Configs _configs;
    private UIRoot _uiRoot;
    private EcsWorld _world;
    private UpgradesController _upgradesController;

    public void Init()
    {
        _uiRoot.Init(_configs);
        _uiRoot.GetPanel<UpgradesPanel>().OnBusinessUpgradeButtonPressed += TryUpgradeBusiness;

        for (int i = 0; i < _configs.Businesses.Count; i++)
        {
            BusinessPanel newBusinessPanel = _uiRoot.GetPanel<UpgradesPanel>().CreateBusinessPanel(_configs.Businesses[i], TryUpgradeBusinessMultiplier);

            EcsEntity newBusinessEntity = _world.NewEntity();

            newBusinessEntity.Get<ViewObject<BusinessPanel>>().View = newBusinessPanel;
            ref var businessData = ref newBusinessEntity.Get<BusinessComponent>();

            businessData.BusinessID = i;
            businessData.UpgradeLevel = i == 0 ? 1 : 0;
            businessData.BusinessData = _configs.Businesses[i];

            ref var businessUpgradesData = ref newBusinessEntity.Get<BusinessMultiplierUpgradesComponent>();
            businessUpgradesData.ActivationMap = new List<bool>();
            businessUpgradesData.Value = new List<BusinessUpgradeDataItem>();
            foreach (var businessUpgradeButton in newBusinessPanel.GetUpgradeButtonsView())
            {
                BusinessUpgradeDataItem dataItem = new BusinessUpgradeDataItem(businessUpgradeButton.GetCurrentData(), businessUpgradeButton);
                businessUpgradesData.ActivationMap.Add(false);
                businessUpgradesData.Value.Add(dataItem);
            }

            newBusinessPanel.UpdateUpgradeLevel(businessData.UpgradeLevel);
            newBusinessPanel.UpdateBusinessUpgradeLevelCost(_upgradesController.GetUpgradeCost(businessData.UpgradeLevel, businessData.BusinessData.DefaultCost));
            newBusinessPanel.UpdateIncomeValue(_upgradesController.GetIncomeValueByBusinessUpgrade(businessData, businessUpgradesData, businessData.BusinessData.DefaultIncome));
        }
    }

    private void TryUpgradeBusiness(BusinessData businessData)
    {
        foreach (var item in _businessFilter)
        {
            if (_businessFilter.Get1(item).BusinessData == businessData)
            {
                _businessFilter.GetEntity(item).Get<UpgradeBusinessLevelTrigger>();
                break;
            }
        }
    }

    private void TryUpgradeBusinessMultiplier(BusinessUpgradeData businessUpgradeData)
    {
        foreach (var item in _businessFilter)
        {
            for (int i = 0; i < _businessFilter.Get2(item).Value.Count; i++)
            {
                if (!_businessFilter.Get2(item).ActivationMap[i] && _businessFilter.Get2(item).Value[i].Data == businessUpgradeData)
                {
                    ref var businessUpgradeTrigger = ref _businessFilter.GetEntity(item).Get<UpgradeBusinessButtonTrigger>();
                    businessUpgradeTrigger.Identificator = businessUpgradeData;
                    return;
                }
            }
        }
    }
}