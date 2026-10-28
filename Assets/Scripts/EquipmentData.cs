using UnityEngine;

public enum EquipmentType
{
    InvestigationTool,
    ExorcismTool,
    MediaTool
}

[CreateAssetMenu(fileName = "NewEquipment", menuName = "Ghost Tycoon/Equipment Data")]
public class EquipmentData : ScriptableObject
{
    public string itemName = "New Item";
    public float cost = 100f; // Added cost for the Shop system
    public EquipmentType type;

    [Header("Buffs & Modifiers")]
    [Tooltip("Bonus to mission success chance in %")]
    public float successChanceBonus = 0f;

    [Tooltip("Multiplier for financial reward (e.g. 1.5 = +50% payout from views)")]
    public float rewardMultiplier = 1.0f;

    [Tooltip("Reduces the risk of employee casualty in %")]
    public float safetyBonus = 0f;
}