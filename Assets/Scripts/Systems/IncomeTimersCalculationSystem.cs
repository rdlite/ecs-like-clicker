using Leopotam.Ecs;
using UnityEngine;

public class IncomeTimersCalculationSystem : IEcsRunSystem
{
    private EcsFilter<BusinessComponent, ViewObject<BusinessPanel>>.Exclude<ClosedBuissinessTagComponent> _businessesFilter;

    public void Run()
    {
        float deltaTime = Time.deltaTime;

        foreach (var item in _businessesFilter)
        {
            ref BusinessComponent businessData = ref _businessesFilter.Get1(item);
            ref ViewObject<BusinessPanel> view = ref _businessesFilter.Get2(item);

            businessData.CurrentIncomeTimer += deltaTime;
            view.View.SetSliderProgression(businessData.CurrentIncomeTimer / businessData.BusinessData.IncomeDelay);

            if (businessData.CurrentIncomeTimer >= businessData.BusinessData.IncomeDelay)
            {
                businessData.CurrentIncomeTimer = 0f;
                _businessesFilter.GetEntity(item).Get<IncomeEventTrigger>();
            }
        }
    }
}