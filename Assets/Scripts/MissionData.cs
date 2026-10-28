using UnityEngine;

public enum MissionType
{
    Investigation,
    Exorcism,
    Filming
}

[CreateAssetMenu(fileName = "NewMission", menuName = "GameData/Mission Data")]
public class MissionData : ScriptableObject
{
    [Header("Mission Info")]
    public string missionTitle = "New Mission";
    public MissionType type = MissionType.Investigation;
    public LocationData location;
    public GhostData ghost;

    [Header("Contract Info")]
    public float baseReward = 100f;
    public bool isEmergencyContract = false;
    public int daysToExpire = 2;

    [Header("Mission State")]
    public bool isGhostIdentified = false;
    public EquipmentData assignedEquipment;

    public float GetTotalReward()
    {
        float finalReward = baseReward;

        // 1. Mission Type Multiplier
        float missionTypeMultiplier = 1.0f;
        switch (type)
        {
            case MissionType.Investigation: 
                missionTypeMultiplier = 1.0f; 
                break;
            case MissionType.Filming: 
                missionTypeMultiplier = 1.3f; 
                break;
            case MissionType.Exorcism: 
                missionTypeMultiplier = 2.0f; 
                break;
        }
        finalReward *= missionTypeMultiplier;

        // 2. Location Difficulty Multiplier
        if (location != null)
        {
            finalReward += location.difficultyScore * 5f;
        }

        // 3. Ghost Threat Multiplier
        if (ghost != null)
        {
            finalReward += ghost.baseDifficulty * 4f;
        }

        // 4. Assigned Equipment Multiplier
        if (assignedEquipment != null)
        {
            finalReward *= assignedEquipment.rewardMultiplier;
        }

        return Mathf.Round(finalReward);
    }
}