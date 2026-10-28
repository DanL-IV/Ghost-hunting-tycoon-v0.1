using UnityEngine;
using System.Collections.Generic;

public class TeamSelectionManager : MonoBehaviour
{
    public static TeamSelectionManager Instance { get; private set; }

    public const int MaxTeamSize = 3;

    [Header("Selection State")]
    public MissionData selectedMission;
    public List<EmployeeData> selectedTeam = new List<EmployeeData>();

    [Header("Assigned Equipment")]
    public EquipmentData selectedEquipment;

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
    }

    public void SelectMission(MissionData mission)
    {
        selectedMission = mission;
        ApplyEquipmentToMission();
    }

    public void SelectEquipment(EquipmentData equipment)
    {
        selectedEquipment = equipment;
        ApplyEquipmentToMission();
    }

    // Equipment lives on the mission, so prediction and real roll use the same data
    private void ApplyEquipmentToMission()
    {
        if (selectedMission != null) selectedMission.assignedEquipment = selectedEquipment;
    }

    public void AddEmployeeToTeam(EmployeeData employee)
    {
        if (employee != null && !selectedTeam.Contains(employee) && selectedTeam.Count < MaxTeamSize)
        {
            selectedTeam.Add(employee);
            Debug.Log($"Added {employee.employeeName} to the team.");
        }
    }

    public void RemoveEmployeeFromTeam(EmployeeData employee)
    {
        if (selectedTeam.Contains(employee))
        {
            selectedTeam.Remove(employee);
            Debug.Log($"Removed {employee.employeeName} from the team.");
        }
    }

    public float CalculatePredictedChance()
    {
        return MissionResolver.CalculateChance(selectedMission, selectedTeam);
    }

    public void ConfirmAndSendMission()
    {
        if (selectedMission == null || selectedTeam.Count == 0) return;

        if (ContractBoard.Instance == null)
        {
            Debug.LogError("[TeamSelection] ContractBoard is missing on the scene!");
            return;
        }

        ContractBoard.Instance.StartMission(selectedMission, new List<EmployeeData>(selectedTeam));
        ClearSelection();
    }

    public void ClearSelection()
    {
        selectedTeam.Clear();
        selectedMission = null;
        selectedEquipment = null;
    }
}