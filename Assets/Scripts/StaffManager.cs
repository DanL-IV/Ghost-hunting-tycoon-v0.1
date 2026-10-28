using UnityEngine;
using System.Collections.Generic;

public class StaffManager : MonoBehaviour
{
    public static StaffManager Instance { get; private set; }

    [Header("Available Candidates Pool")]
    public List<EmployeeData> availableCandidates = new List<EmployeeData>();

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

    public bool HireEmployee(EmployeeData candidate, float hireCost)
    {
        if (candidate == null || HQManager.Instance == null) return false;

        if (!HQManager.Instance.CanHireNewEmployee())
        {
            Debug.LogWarning("Cannot hire: HQ employee capacity reached!");
            return false;
        }

        if (!HQManager.Instance.SpendMoney(hireCost)) return false;

        // Hire a runtime COPY: stress and other changes must not be written into the asset
        EmployeeData hired = Instantiate(candidate);
        HQManager.Instance.hiredEmployees.Add(hired);
        availableCandidates.Remove(candidate);

        Debug.Log($"Hired {hired.employeeName} for ${hireCost}!");
        return true;
    }
}