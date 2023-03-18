using UnityEngine;

[CreateAssetMenu(fileName = "New business upgrade", menuName = "Configs/Business upgrade data")]
public class BusinessUpgradeData : ScriptableObject
{
    public string Title;
    [Header("Measure: percentage")]
    public float IncomeMultiplier = 50f;
    public float Cost = 100;
}