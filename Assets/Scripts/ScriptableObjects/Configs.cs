using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Configs", menuName = "Configs/Main configs")]
public class Configs : ScriptableObject
{
    public List<BusinessData> Businesses;
    public float BackgroundSavingTimer = 5f;

    [Header("UI configs")]
    public float DeactivatedBusinessPanelAlpha = .6f;

    [Header("Configs and containers")]
    public PrefabsContainer PrefabsContainer;
}