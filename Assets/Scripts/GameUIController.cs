using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GameUIController : MonoBehaviour
{
    [Header("UI Text Elements (TextMeshPro)")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI ghostText;
    public TextMeshProUGUI successChanceText;

    [Header("Testing References (debug quick-send only)")]
    public MissionData targetMission;
    public List<EmployeeData> testTeam = new List<EmployeeData>();

    private void OnEnable()
    {
        ContractBoard.OnMissionCompleted += ShowResult;
    }

    private void OnDisable()
    {
        ContractBoard.OnMissionCompleted -= ShowResult;
    }

    private void Update()
    {
        UpdateTopBar();
    }

    // NOTE: UIManager also updates a top bar. Use only one of them in the final scene.
    private void UpdateTopBar()
    {
        if (HQManager.Instance != null)
        {
            if (moneyText != null) moneyText.text = $"Money: ${HQManager.Instance.currentMoney}";
            if (dayText != null) dayText.text = $"Day: {HQManager.Instance.currentDay}";
        }
    }

    // Shows the result of ANY mission, no matter which UI sent it
    private void ShowResult(MissionData mission, MissionResolver.MissionResult result)
    {
        if (resultText != null) resultText.text = result.resultLog;
        if (successChanceText != null) successChanceText.text = $"Chance was: {result.finalSuccessChance:F0}%";
        if (ghostText != null && mission != null && mission.ghost != null)
            ghostText.text = $"Ghost: {mission.ghost.ghostName}";
    }

    // Debug quick-send: first contract + all hired employees
    public void OnClickSendMission()
    {
        if (ContractBoard.Instance == null || HQManager.Instance == null)
        {
            Debug.LogError("[UI] Managers are missing on the scene!");
            return;
        }

        if (ContractBoard.Instance.availableContracts.Count == 0)
        {
            if (resultText != null) resultText.text = "No available contracts!";
            return;
        }

        MissionData mission = ContractBoard.Instance.availableContracts[0];
        List<EmployeeData> team = HQManager.Instance.hiredEmployees.Count > 0
            ? new List<EmployeeData>(HQManager.Instance.hiredEmployees)
            : new List<EmployeeData>(testTeam);

        ContractBoard.Instance.StartMission(mission, team);

        // Do not keep a stale reference to a contract that no longer exists
        targetMission = null;
    }
}