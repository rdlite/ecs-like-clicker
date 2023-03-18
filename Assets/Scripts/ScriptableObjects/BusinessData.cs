using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New business data", menuName = "Configs/Business data")]
public class BusinessData : ScriptableObject
{
    public string Title;
    public float IncomeDelay = 3f;
    public float DefaultIncome = 3f;
    public float DefaultCost = 3f;
    public List<BusinessUpgradeData> UpgradeButtons;
}