using UnityEngine;
using System.Collections.Generic;

public class HQManager : MonoBehaviour
{
    public static HQManager Instance { get; private set; }

    [Header("Economy & Time")]
    public float currentMoney = 1000f;
    public int currentDay = 1;

    [Header("Company Reputation")]
    [Range(0f, 100f)] public float companyReputation = 50f;
    public float psychCost = 150f;

    [Header("HQ Capacity")]
    public int maxEmployeeCapacity = 3;

    [Header("Active Roster & Storage")]
    public List<EmployeeData> hiredEmployees = new List<EmployeeData>();
    public List<EquipmentData> inventoryEquipment = new List<EquipmentData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Starting employees assigned in the Inspector are assets: use runtime copies
        // so stress and other changes never get written into the assets
        for (int i = 0; i < hiredEmployees.Count; i++)
        {
            if (hiredEmployees[i] != null) hiredEmployees[i] = Instantiate(hiredEmployees[i]);
        }
    }

    public bool CanHireNewEmployee()
    {
        return hiredEmployees.Count < maxEmployeeCapacity;
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        Debug.Log($"[HQ] Added ${amount}. Total: ${currentMoney}");
    }

    public bool SpendMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log($"[HQ] Spent ${amount}. Remaining: ${currentMoney}");
            return true;
        }
        Debug.LogWarning("[HQ] Not enough money!");
        return false;
    }

    public void SendToPsychologist(EmployeeData employee)
    {
        if (employee == null) return;

        if (SpendMoney(psychCost))
        {
            employee.stressLevel = 0f;
            Debug.Log($"[HQ] Employee {employee.employeeName} visited the psychologist. Stress reset to 0.");
        }
    }

    public void ModifyReputation(float amount)
    {
        companyReputation = Mathf.Clamp(companyReputation + amount, 0f, 100f);
        Debug.Log($"[HQ] Company Reputation updated: {companyReputation}");
    }

    public void ProcessEndOfDay()
    {
        currentDay++;
        Debug.Log($"--- Processing End of Day {currentDay} ---");

        float totalSalaries = 0f;
        foreach (var emp in hiredEmployees)
        {
            if (emp != null) totalSalaries += emp.dailySalary;
        }

        if (SpendMoney(totalSalaries))
        {
            Debug.Log($"[HQ] Paid total daily salaries: ${totalSalaries}");
        }
        else
        {
            Debug.LogWarning("[HQ] Cannot pay full salaries! Company reputation decreased.");
            ModifyReputation(-5f);
        }

        for (int i = hiredEmployees.Count - 1; i >= 0; i--)
        {
            var emp = hiredEmployees[i];
            if (emp != null)
            {
                if (emp.stressLevel >= 91f)
                {
                    Debug.LogWarning($"[HQ] {emp.employeeName} suffered a nervous breakdown and resigned!");
                    hiredEmployees.RemoveAt(i);
                    ModifyReputation(-3f);
                }
                else
                {
                    emp.stressLevel = Mathf.Max(0f, emp.stressLevel - 15f);
                }
            }
        }

        // New day, new contracts
        if (ContractBoard.Instance != null) ContractBoard.Instance.GenerateDailyContracts();
    }
}