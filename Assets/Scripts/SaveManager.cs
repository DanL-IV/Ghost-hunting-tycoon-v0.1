using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class GameData
{
    public float currentMoney;
    public int currentDay;
    public float companyReputation;
    public int maxEmployeeCapacity;

    public int hqLevel;
    public int missionAreaLevel;
    public int communicationLevel;

    public List<string> discoveredGhosts = new List<string>();
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Define path for JSON save file
        saveFilePath = Path.Combine(Application.persistentDataPath, "ghost_tycoon_save.json");
    }

    // Save all key game states
    public void SaveGame()
    {
        GameData data = new GameData();

        // Save HQ & Economy data
        if (HQManager.Instance != null)
        {
            data.currentMoney = HQManager.Instance.currentMoney;
            data.currentDay = HQManager.Instance.currentDay;
            data.companyReputation = HQManager.Instance.companyReputation;
            data.maxEmployeeCapacity = HQManager.Instance.maxEmployeeCapacity;
        }

        // Save Base Upgrades data
        if (BaseUpgradeManager.Instance != null)
        {
            data.hqLevel = BaseUpgradeManager.Instance.hqLevel;
            data.missionAreaLevel = BaseUpgradeManager.Instance.missionAreaLevel;
            data.communicationLevel = BaseUpgradeManager.Instance.communicationLevel;
        }

        // Save Bestiary data
        if (BestiaryManager.Instance != null)
        {
            data.discoveredGhosts = new List<string>(BestiaryManager.Instance.discoveredGhostTypes);
        }

        // Convert to JSON and write to file
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"[SaveManager] Game successfully saved to: {saveFilePath}");
    }

    // Load game states back into managers
    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("[SaveManager] Save file not found! Starting fresh.");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        // Restore HQ & Economy data
        if (HQManager.Instance != null)
        {
            HQManager.Instance.currentMoney = data.currentMoney;
            HQManager.Instance.currentDay = data.currentDay;
            HQManager.Instance.companyReputation = data.companyReputation;
            HQManager.Instance.maxEmployeeCapacity = data.maxEmployeeCapacity;
        }

        // Restore Base Upgrades data
        if (BaseUpgradeManager.Instance != null)
        {
            BaseUpgradeManager.Instance.hqLevel = data.hqLevel;
            BaseUpgradeManager.Instance.missionAreaLevel = data.missionAreaLevel;
            BaseUpgradeManager.Instance.communicationLevel = data.communicationLevel;
        }

        // Restore Bestiary data
        if (BestiaryManager.Instance != null)
        {
            BestiaryManager.Instance.discoveredGhostTypes = new List<string>(data.discoveredGhosts);
        }

        Debug.Log("[SaveManager] Game successfully loaded from save file!");
    }

    // Optional: Delete save file
    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("[SaveManager] Save file deleted.");
        }
    }
}