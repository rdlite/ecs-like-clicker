using Leopotam.Ecs;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelsSystem : IEcsRunSystem
{
    private EcsFilter<LoadLevelTrigger> _levelTrigger;
    private ICoroutineRunner _coroutineRunner;

    public void Run()
    {
        foreach (var item in _levelTrigger)
        {
            StartLoadLevel(_levelTrigger.Get1(item).LevelID);
        }
    }

    private void StartLoadLevel(int id)
    {
        _coroutineRunner.StartCoroutine(AsyncSceneLoading(id));
    }

    private IEnumerator AsyncSceneLoading(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}