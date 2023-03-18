using Leopotam.Ecs;
using UnityEngine;

public class UpgradeBusinessSystem : IEcsRunSystem
{
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, ViewObject<BusinessPanel>> _businessFilters;
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, ViewObject<BusinessPanel>, SetUpgradeLevelTrigger> _setUpgradeInstantlyFilter;
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, ViewObject<BusinessPanel>, SetUpgradeMultiplierTrigger> _setUpgradeMultiplierFilter;
    private UpgradesController _upgradesController;

    public void Run()
    {
        foreach (var item in _businessFilters)
        {
            if (_businessFilters.GetEntity(item).Has<UpgradeBusinessLevelTrigger>())
            {
                bool isUpgraded = _upgradesController.UpgradeBusiness(ref _businessFilters.Get1(item), _businessFilters.Get2(item), _businessFilters.Get3(item));

                if (isUpgraded)
                {
                    _businessFilters.GetEntity(item).Del<UpgradeBusinessLevelTrigger>();
                    _businessFilters.GetEntity(item).Del<ClosedBuissinessTagComponent>();
                    ActivateNextBusinessView(item);
                }
            }

            if (_businessFilters.GetEntity(item).Has<UpgradeBusinessButtonTrigger>() && !_businessFilters.GetEntity(item).Has<ClosedBuissinessTagComponent>())
            {
                _upgradesController.UpgradeBusinessMuiltiplier(
                     _businessFilters.Get1(item), ref _businessFilters.Get2(item),
                     _businessFilters.Get3(item), _businessFilters.GetEntity(item).Get<UpgradeBusinessButtonTrigger>().Identificator);

                _businessFilters.GetEntity(item).Del<UpgradeBusinessButtonTrigger>();
            }
        }
        
        foreach (var item in _setUpgradeInstantlyFilter)
        {
            _upgradesController.SetBusinessUpgradeLevel(
                ref _businessFilters.Get1(item), _businessFilters.Get2(item), _businessFilters.Get3(item), 
                _setUpgradeInstantlyFilter.Get4(item).UpgradeID);
        }
        
        foreach (var item in _setUpgradeMultiplierFilter)
        {
            for (int i = 0; i < _setUpgradeMultiplierFilter.Get4(item).UpgradeItems.Count; i++)
            {
                _upgradesController.ActivateBusinessMultiplier(
                     _setUpgradeMultiplierFilter.Get1(item), ref _setUpgradeMultiplierFilter.Get2(item),
                     _setUpgradeMultiplierFilter.Get3(item), _setUpgradeMultiplierFilter.Get4(item).UpgradeItems[i].Data);
            }
        }
    }

    private void ActivateNextBusinessView(int filterID)
    {
        if ((filterID + 1) < _businessFilters.GetEntitiesCount())
        {
            filterID++;
            _businessFilters.Get3(filterID).View.SetEnable(true);
        }
    }
}