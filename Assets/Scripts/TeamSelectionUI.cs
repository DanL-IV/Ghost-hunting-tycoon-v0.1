using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TeamSelectionUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI selectedMissionText;
    public TextMeshProUGUI predictedChanceText;
    public Button sendMissionButton;

    [Header("Team Slot UI Displays (Slots 1 to 3)")]
    public List<TextMeshProUGUI> slotTexts;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (TeamSelectionManager.Instance == null) return;

        // Update selected mission info
        var mission = TeamSelectionManager.Instance.selectedMission;
        if (selectedMissionText != null)
        {
            if (mission != null && mission.location != null && mission.ghost != null)
            {
                selectedMissionText.text = $"Target: {mission.location.locationName} vs {mission.ghost.ghostName}";
            }
            else
            {
                selectedMissionText.text = "No Mission Selected";
            }
        }

        // Update predicted success chance
        if (predictedChanceText != null && mission != null)
        {
            float chance = TeamSelectionManager.Instance.CalculatePredictedChance();
            predictedChanceText.text = $"Success Chance: {chance:F1}%";
        }
        else if (predictedChanceText != null)
        {
            predictedChanceText.text = "Success Chance: --";
        }

        // Update team slot texts
        var team = TeamSelectionManager.Instance.selectedTeam;
        for (int i = 0; i < slotTexts.Count; i++)
        {
            if (slotTexts[i] != null)
            {
                if (i < team.Count && team[i] != null)
                {
                    slotTexts[i].text = $"{team[i].employeeName}\n<size=80%>{team[i].role} (Stress: {team[i].stressLevel}%)</size>";
                }
                else
                {
                    slotTexts[i].text = $"Slot {i + 1}\n[Empty]";
                }
            }
        }

        // Enable or disable send button
        if (sendMissionButton != null)
        {
            sendMissionButton.interactable = (mission != null && team.Count > 0);
        }
    }

    public void OnClickAddEmployee(EmployeeData employee)
    {
        if (TeamSelectionManager.Instance != null)
        {
            TeamSelectionManager.Instance.AddEmployeeToTeam(employee);
        }
    }

    public void OnClickConfirmMission()
    {
        if (TeamSelectionManager.Instance != null)
        {
            TeamSelectionManager.Instance.ConfirmAndSendMission();
        }
    }
}