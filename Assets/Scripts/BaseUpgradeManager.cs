using UnityEngine;

public class BaseUpgradeManager : MonoBehaviour
{
    public static BaseUpgradeManager Instance { get; private set; }

    [Header("Base Levels & Costs")]
    public int hqLevel = 1;
    public float hqUpgradeCost = 500f;

    public int missionAreaLevel = 1;
    public float missionAreaUpgradeCost = 400f;

    public int communicationLevel = 1;
    public float communicationUpgradeCost = 300f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpgradeHQCapacity()
    {
        if (HQManager.Instance != null && HQManager.Instance.SpendMoney(hqUpgradeCost))
        {
            hqLevel++;
            HQManager.Instance.maxEmployeeCapacity += 2; // Expand max roster slots
            hqUpgradeCost *= 1.5f;
            Debug.Log($"[Base] HQ upgraded to Level {hqLevel}. Max capacity is now {HQManager.Instance.maxEmployeeCapacity}");
        }
    }

    public void UpgradeMissionArea()
    {
        if (HQManager.Instance != null && HQManager.Instance.SpendMoney(missionAreaUpgradeCost))
        {
            missionAreaLevel++;
            missionAreaUpgradeCost *= 1.5f;
            Debug.Log($"[Base] Mission Area expanded to Level {missionAreaLevel}. Higher tier contracts unlocked!");
        }
    }

    public void UpgradeCommunication()
    {
        if (HQManager.Instance != null && HQManager.Instance.SpendMoney(communicationUpgradeCost))
        {
            communicationLevel++;
            communicationUpgradeCost *= 1.5f;
            Debug.Log($"[Base] Communication & Training upgraded to Level {communicationLevel}. Stress recovery improved.");
        }
    }
}