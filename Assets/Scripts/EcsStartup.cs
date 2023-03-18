using Leopotam.Ecs;
using UnityEngine;

public sealed class EcsStartup : MonoBehaviour
{
    private EcsWorld _world;
    private EcsSystems _systems;

    public void Init(
        UIRoot uiRoot, Configs configs, MoneyFlowResolver moneyFlowResolver,
        UpgradesController upgradesController, ICoroutineRunner coroutineRunner)
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world);

#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create(_systems);
#endif
        _systems
            .Add(new InitUISystem())
            .Add(new LoadSaveDataSystem())
            .Add(new LoadLevelsSystem())
            .Add(new IncomeTimersCalculationSystem())
            .Add(new IncomeEventsResolveSystem())
            .Add(new UpgradeBusinessSystem())

            .OneFrame<IncomeEventTrigger>()
            .OneFrame<UpgradeBusinessLevelTrigger>()
            .OneFrame<UpgradeBusinessButtonTrigger>()
            .OneFrame<LoadLevelTrigger>()
            .OneFrame<SetUpgradeLevelTrigger>()
            .OneFrame<SetUpgradeMultiplierTrigger>()

            .Inject(configs)
            .Inject(uiRoot)
            .Inject(moneyFlowResolver)
            .Inject(upgradesController)
            .Inject(coroutineRunner)

            .Init();
    }

    public void Run()
    {
        _systems?.Run();
    }

    private void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
            _world.Destroy();
            _world = null;
        }
    }
}