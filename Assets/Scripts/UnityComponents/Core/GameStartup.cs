using UnityEngine;

public class GameStartup : MonoBehaviour, ICoroutineRunner
{
    [SerializeField] private Configs _configs;
    [SerializeField] private EcsStartup _ecsStartup;
    [SerializeField] private UIRoot _uiRoot;
    [SerializeField] private Camera _mainCamera;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        _uiRoot = Instantiate(_uiRoot);
        _mainCamera = Instantiate(_mainCamera);

        UpgradesController upgradesController = new UpgradesController();
        MoneyFlowResolver moneyFlowResolver = new MoneyFlowResolver();
        moneyFlowResolver.Init(_uiRoot);
        upgradesController.Init(moneyFlowResolver);

        _ecsStartup.Init(
            _uiRoot, _configs, moneyFlowResolver,
            upgradesController, this);
    }

    private void Update()
    {
        _ecsStartup.Run();

        // простой чит на скорость игры для дебага
#if UNITY_EDITOR 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = Time.timeScale == 5f ? 1f : 5f;
        }
#endif
    }
}