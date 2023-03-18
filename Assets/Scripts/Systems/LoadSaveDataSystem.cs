using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveDataSystem : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter<BusinessComponent, BusinessMultiplierUpgradesComponent, ViewObject<BusinessPanel>> _businessesFilter;
    private Configs _configs;
    private UIRoot _uiRoot;
    private EcsWorld _world;
    private MoneyFlowResolver _moneyFlowResolver;

    private float _savingTimer;

    public void Init()
    {
        bool hasSave = PlayerPrefs.HasKey(PrefsContainer.HAS_SAVE_KEYS);

        if (!hasSave)
        {
            for (int i = 0; i < _configs.Businesses.Count; i++)
            {
                _uiRoot.GetPanel<UpgradesPanel>().SetBusinessPanelActive(_configs.Businesses[i], i <= 1);

                foreach (var item in _businessesFilter)
                {
                    if (_businessesFilter.Get1(item).BusinessID == i)
                    {
                        if (i != 0)
                        {
                            _businessesFilter.GetEntity(item).Get<ClosedBuissinessTagComponent>();
                        }

                        continue;
                    }
                }
            }
        }
        else
        {
            int id = 0;
            foreach (var item in _uiRoot.GetPanel<UpgradesPanel>().GetBusinessPanels())
            {
                ref var businessData = ref _businessesFilter.Get1(id);

                float savedProgressionTimer = PlayerPrefs.GetFloat(businessData.BusinessID + PrefsContainer.BUSINESS_PROGRESS_KEY);
                businessData.CurrentIncomeTimer = savedProgressionTimer * businessData.BusinessData.IncomeDelay;
                item.SetSliderProgression(savedProgressionTimer);

                // восстановление уровня прокачки бизнеса
                int upgradeID = PlayerPrefs.GetInt(businessData.BusinessID + PrefsContainer.BUSINESS_LEVEL_KEY);
                if (upgradeID == 0)
                {
                    _businessesFilter.GetEntity(id).Get<ClosedBuissinessTagComponent>();

                    // отрабатывает, чтобы включить визуал еще непрокачанного бизнеса, если предыдущий открыт
                    if (_businessesFilter.Get1(id - 1).UpgradeLevel == 0 && id != 0)
                    {
                        _businessesFilter.Get3(id).View.SetEnable(false);
                    }
                }
                else
                {
                    businessData.UpgradeLevel = upgradeID;
                    _businessesFilter.GetEntity(id).Get<SetUpgradeLevelTrigger>().UpgradeID = upgradeID;

                    // восстановление множителей бизнеса
                    for (int i = 0; i < _businessesFilter.Get2(id).ActivationMap.Count; i++)
                    {
                        bool isActive = PlayerPrefs.GetInt(businessData.BusinessID + "" + i + PrefsContainer.BUSINESS_MULTIPLIER_UPGRADE_KEY) == 1;

                        if (isActive)
                        {
                            ref var dataToUpgrade = ref _businessesFilter.GetEntity(id).Get<SetUpgradeMultiplierTrigger>();
                            if (dataToUpgrade.UpgradeItems == null)
                            {
                                dataToUpgrade.UpgradeItems = new List<BusinessUpgradeDataItem>();
                            }
                            dataToUpgrade.UpgradeItems.Add(_businessesFilter.Get2(id).Value[i]);
                        }
                    }
                }

                id++;
            }
        }

        _moneyFlowResolver.SetMoneyAmount(PlayerPrefs.GetFloat(PrefsContainer.SOFT_MONEY_AMOUNT_KEY));
        _world.NewEntity().Get<LoadLevelTrigger>().LevelID = 1;
    }

    public void Run()
    {
        _savingTimer += Time.deltaTime;

        if (_savingTimer >= _configs.BackgroundSavingTimer)
        {
            SaveGameState();
            _savingTimer = 0f;
        }
    }

    private void SaveGameState()
    {
        PlayerPrefs.SetInt(PrefsContainer.HAS_SAVE_KEYS, 1);

        PlayerPrefs.SetFloat(PrefsContainer.SOFT_MONEY_AMOUNT_KEY, _moneyFlowResolver.GetMoneyAmount());

        foreach (var item in _businessesFilter)
        {
            var businessData = _businessesFilter.Get1(item);
            PlayerPrefs.SetFloat(businessData.BusinessID + PrefsContainer.BUSINESS_PROGRESS_KEY, businessData.CurrentIncomeTimer / businessData.BusinessData.IncomeDelay);
            PlayerPrefs.SetInt(businessData.BusinessID + PrefsContainer.BUSINESS_LEVEL_KEY, businessData.UpgradeLevel);

            int multiplierID = 0;
            foreach (var multiplierActivation in _businessesFilter.Get2(item).ActivationMap)
            {
                PlayerPrefs.SetInt(businessData.BusinessID + "" + multiplierID + PrefsContainer.BUSINESS_MULTIPLIER_UPGRADE_KEY, multiplierActivation ? 1 : 0);
                multiplierID++;
            }
        }
    }
}