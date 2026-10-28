using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HQPanelUI : MonoBehaviour
{
    [Header("UI References")]
    public Transform employeeListContentParent; // Content container in ScrollView for employee cards
    public GameObject employeeCardPrefab;       // Prefab representing a single employee row/card
    public TextMeshProUGUI totalEmployeesText;

    private void OnEnable()
    {
        RefreshHQUI();
    }

    public void RefreshHQUI()
    {
        if (employeeListContentParent == null || employeeCardPrefab == null) return;

        // Clear existing UI cards
        foreach (Transform child in employeeListContentParent)
        {
            Destroy(child.gameObject);
        }

        if (HQManager.Instance == null || HQManager.Instance.hiredEmployees == null) return;

        var employees = HQManager.Instance.hiredEmployees;

        if (totalEmployeesText != null)
        {
            totalEmployeesText.text = "Hired Personnel: " + employees.Count;
        }

        // Instantiate cards for each hired employee
        foreach (var emp in employees)
        {
            GameObject cardObj = Instantiate(employeeCardPrefab, employeeListContentParent);
            
            // Look for text and button components inside the card prefab
            TextMeshProUGUI cardText = cardObj.GetComponentInChildren<TextMeshProUGUI>();
            Button restButton = cardObj.transform.Find("RestButton")?.GetComponent<Button>();

            if (cardText != null)
            {
                cardText.text = emp.employeeName + " (" + emp.role + ")\n" +
                                "Inv: " + emp.investigationSkill + " | Exorcism: " + emp.exorcismSkill + " | Rec: " + emp.recordingSkill + "\n" +
                                "Stress: " + emp.stressLevel + "%";
            }

            if (restButton != null)
            {
                EmployeeData targetEmp = emp; // Closure capture fix
                restButton.onClick.AddListener(() => OnRestEmployeeClicked(targetEmp));
            }
        }
    }

    private void OnRestEmployeeClicked(EmployeeData employee)
    {
        if (employee != null)
        {
            // Reduce stress by resting (e.g., -30%)
            employee.stressLevel = Mathf.Clamp(employee.stressLevel - 30f, 0f, 100f);
            Debug.Log(employee.employeeName + " rested. Current stress: " + employee.stressLevel + "%");
            RefreshHQUI();
        }
    }
}