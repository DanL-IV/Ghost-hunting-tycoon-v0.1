using UnityEngine;
using System.Collections.Generic;

public class ContractBoard : MonoBehaviour
{
    public static ContractBoard Instance { get; private set; }

    // UI can subscribe to refresh itself / show the result
    public static event System.Action<MissionData, MissionResolver.MissionResult> OnMissionCompleted;

    [Header("Master Data Pools")]
    public List<LocationData> allLocations = new List<LocationData>();
    public List<GhostData> allGhosts = new List<GhostData>();

    [Header("Active Daily Contracts")]
    public List<MissionData> availableContracts = new List<MissionData>();

    public MissionResolver.MissionResult LastResult { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (availableContracts.Count == 0) GenerateDailyContracts();
    }

    public void GenerateDailyContracts(int count = 3)
    {
        availableContracts.Clear();

        if (allLocations == null || allLocations.Count == 0 || allGhosts == null || allGhosts.Count == 0)
        {
            Debug.LogWarning("[ContractBoard] Location or Ghost pools are empty!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            MissionData newMission = ScriptableObject.CreateInstance<MissionData>();
            newMission.location = allLocations[Random.Range(0, allLocations.Count)];
            newMission.ghost = allGhosts[Random.Range(0, allGhosts.Count)];
            newMission.type = (MissionType)Random.Range(0, 3);
            newMission.missionTitle = $"{newMission.type}: {newMission.location.locationName}";
            newMission.baseReward = Random.Range(200f, 600f);

            availableContracts.Add(newMission);
        }

        Debug.Log($"[ContractBoard] Generated {availableContracts.Count} new contracts.");
    }

    // THE ONLY way to run a mission. Every UI must call this.
    public MissionResolver.MissionResult StartMission(MissionData mission, List<EmployeeData> team)
    {
        MissionResolver.MissionResult failed = new MissionResolver.MissionResult();
        failed.casualties = new List<EmployeeData>();

        if (mission == null)
        {
            failed.resultLog = "Cannot start: mission is null.";
            Debug.LogWarning("[ContractBoard] " + failed.resultLog);
            return failed;
        }

        if (HQManager.Instance == null)
        {
            failed.resultLog = "Cannot start: HQManager is missing.";
            Debug.LogError("[ContractBoard] " + failed.resultLog);
            return failed;
        }

        if (team == null || team.Count == 0)
        {
            failed.resultLog = "Cannot start: no team assigned.";
            Debug.LogWarning("[ContractBoard] " + failed.resultLog);
            return failed;
        }

        // Work on a copy so the caller's list is never modified
        var teamCopy = new List<EmployeeData>(team);

        MissionResolver.MissionResult result = MissionResolver.ResolveMission(mission, teamCopy);
        Debug.Log($"[Mission Log] {result.resultLog} (Chance: {result.finalSuccessChance}%)");

        if (result.isSuccess)
        {
            HQManager.Instance.AddMoney(result.finalReward);

            if (BestiaryManager.Instance != null && mission.ghost != null)
                BestiaryManager.Instance.DiscoverGhost(mission.ghost.ghostName);
        }

        availableContracts.Remove(mission);

        LastResult = result;
        OnMissionCompleted?.Invoke(mission, result);
        return result;
    }
}