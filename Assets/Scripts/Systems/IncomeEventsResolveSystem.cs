using Leopotam.Ecs;
using UnityEngine;

public class IncomeEventsResolveSystem : IEcsRunSystem
{
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, IncomeEventTrigger>.Exclude<ClosedBuissinessTagComponent> _businessesFilter;
    private MoneyFlowResolver _moneyFlowResolver;
    private UpgradesController _upgradesController;

    public void Run()
    {
        foreach (var item in _businessesFilter)
        {
            ref BusinessComponent businessData = ref _businessesFilter.Get1(item);
            float moneyToAdd = _upgradesController.GetIncomeValueByBusinessUpgrade(
                businessData, _businessesFilter.Get2(item), businessData.BusinessData.DefaultIncome);
            _moneyFlowResolver.AddMoney(moneyToAdd);
        }
    }
}