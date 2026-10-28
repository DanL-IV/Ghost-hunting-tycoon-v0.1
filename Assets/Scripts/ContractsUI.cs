using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ContractsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI missionTypeText;   // optional
    public TextMeshProUGUI locationNameText;
    public TextMeshProUGUI ghostTypeText;
    public TextMeshProUGUI rewardText;
    public Button deployButton;               // now means "Assign team"

    [Header("Optional")]
    public GameObject teamSelectionPanel;     // opened when a contract is chosen

    private int currentIndex = 0;

    private void OnEnable()
    {
        currentIndex = 0;
        ContractBoard.OnMissionCompleted += HandleMissionCompleted;
        RefreshContractUI();
    }

    private void OnDisable()
    {
        ContractBoard.OnMissionCompleted -= HandleMissionCompleted;
    }

    private void HandleMissionCompleted(MissionData mission, MissionResolver.MissionResult result)
    {
        RefreshContractUI();
    }

    public void OnNextContract()
    {
        currentIndex++;
        RefreshContractUI();
    }

    public void OnPreviousContract()
    {
        currentIndex--;
        RefreshContractUI();
    }

    public void RefreshContractUI()
    {
        if (ContractBoard.Instance == null) return;

        var contracts = ContractBoard.Instance.availableContracts;

        if (contracts != null && contracts.Count > 0)
        {
            // Wrap the index so Next/Previous loop through contracts
            if (currentIndex >= contracts.Count) currentIndex = 0;
            if (currentIndex < 0) currentIndex = contracts.Count - 1;

            MissionData m = contracts[currentIndex];

            if (missionTypeText != null) missionTypeText.text = $"Type: {m.type}";

            if (locationNameText != null && m.location != null)
                locationNameText.text = $"Location: {m.location.locationName}";

            if (ghostTypeText != null && m.ghost != null)
                ghostTypeText.text = $"Target: {m.ghost.ghostName}";

            if (rewardText != null)
                rewardText.text = $"Reward: ${m.GetTotalReward()}";

            if (deployButton != null) deployButton.interactable = true;
        }
        else
        {
            if (missionTypeText != null) missionTypeText.text = "-";
            if (locationNameText != null) locationNameText.text = "No active contracts";
            if (ghostTypeText != null) ghostTypeText.text = "-";
            if (rewardText != null) rewardText.text = "-";
            if (deployButton != null) deployButton.interactable = false;
        }
    }

    // Hook to the contract button: choose the contract and move to team selection.
    // The mission itself is sent from TeamSelectionUI.OnClickConfirmMission.
    public void OnDeployButtonClicked()
    {
        if (ContractBoard.Instance == null || TeamSelectionManager.Instance == null)
        {
            Debug.LogError("[ContractsUI] ContractBoard or TeamSelectionManager is missing on the scene!");
            return;
        }

        var contracts = ContractBoard.Instance.availableContracts;
        if (contracts == null || contracts.Count == 0) return;

        currentIndex = Mathf.Clamp(currentIndex, 0, contracts.Count - 1);

        TeamSelectionManager.Instance.ClearSelection();
        TeamSelectionManager.Instance.SelectMission(contracts[currentIndex]);

        if (teamSelectionPanel != null) teamSelectionPanel.SetActive(true);
    }
}