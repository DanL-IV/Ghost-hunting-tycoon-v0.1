using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class RecruitmentPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform candidatesContentParent; // Content object with Vertical Layout Group
    public GameObject candidateCardPrefab;     // Prefab for candidate display
    public TextMeshProUGUI availableCountText;

    private void OnEnable()
    {
        RefreshRecruitmentUI();
    }

    public void RefreshRecruitmentUI()
    {
        if (candidatesContentParent == null || candidateCardPrefab == null) return;

        // Clear previous cards
        foreach (Transform child in candidatesContentParent)
        {
            Destroy(child.gameObject);
        }

        if (StaffManager.Instance == null || StaffManager.Instance.availableCandidates == null) return;

        var candidates = StaffManager.Instance.availableCandidates;

        if (availableCountText != null)
        {
            availableCountText.text = "Available Candidates: " + candidates.Count;
        }

        foreach (var candidate in candidates)
        {
            GameObject cardObj = Instantiate(candidateCardPrefab, candidatesContentParent);

            TextMeshProUGUI cardText = cardObj.GetComponentInChildren<TextMeshProUGUI>();
            Button hireButton = cardObj.transform.Find("HireButton")?.GetComponent<Button>();

            // Calculate hiring cost based on total skill sum
            float hireCost = (candidate.investigationSkill + candidate.exorcismSkill + candidate.recordingSkill) * 10f;

            if (cardText != null)
            {
                cardText.text = candidate.employeeName + " (" + candidate.role + ")\n" +
                                "Inv: " + candidate.investigationSkill + " | Exorcism: " + candidate.exorcismSkill + " | Rec: " + candidate.recordingSkill + "\n" +
                                "Cost: $" + hireCost;
            }

            if (hireButton != null)
            {
                EmployeeData targetCandidate = candidate;
                hireButton.onClick.AddListener(() => {
                    if (StaffManager.Instance.HireEmployee(targetCandidate, hireCost))
                    {
                        RefreshRecruitmentUI();
                        // Refresh HQ panel if present
                        HQPanelUI hqUI = FindAnyObjectByType<HQPanelUI>();
                        if (hqUI != null) hqUI.RefreshHQUI();
                    }
                });
            }
        }
    }
}