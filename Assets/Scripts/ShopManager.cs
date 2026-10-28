using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    [Header("Available Roster for Hire")]
    public List<EmployeeData> availableCandidates = new List<EmployeeData>();

    [Header("Available Shop Items")]
    public List<EquipmentData> equipmentCatalog = new List<EquipmentData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool TryHireEmployee(EmployeeData candidate)
    {
        if (candidate == null) return false;

        if (HQManager.Instance == null)
        {
            Debug.LogError("[Shop] HQManager Instance is missing!");
            return false;
        }

        if (!HQManager.Instance.CanHireNewEmployee())
        {
            Debug.LogWarning("[Shop] Cannot hire: HQ employee capacity reached!");
            return false;
        }

        if (HQManager.Instance.SpendMoney(candidate.hireCost))
        {
            HQManager.Instance.hiredEmployees.Add(candidate);

            if (availableCandidates.Contains(candidate))
            {
                availableCandidates.Remove(candidate);
            }

            Debug.Log($"[Shop] Successfully processed hiring for {candidate.employeeName}.");
            return true;
        }

        return false;
    }

    public bool TryBuyEquipment(EquipmentData equipment)
    {
        if (equipment == null) return false;

        if (HQManager.Instance == null)
        {
            Debug.LogError("[Shop] HQManager Instance is missing!");
            return false;
        }

        // Fixed: using 'cost' instead of 'equipmentCost'
        float itemCost = equipment.cost; 

        if (HQManager.Instance.SpendMoney(itemCost))
        {
            HQManager.Instance.inventoryEquipment.Add(equipment);

            if (equipmentCatalog.Contains(equipment))
            {
                equipmentCatalog.Remove(equipment);
            }

            Debug.Log($"[Shop] Successfully processed equipment purchase.");
            return true;
        }

        return false;
    }
}