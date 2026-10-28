using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject hqPanel;
    public GameObject contractsPanel;
    public GameObject shopPanel;

    [Header("TopBar Texts")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI reputationText;

    private void Update()
    {
        UpdateTopBar();
    }

    // Method to switch tabs (assigned to NavBar buttons)
    public void OpenHQ()
    {
        hqPanel.SetActive(true);
        contractsPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OpenContracts()
    {
        hqPanel.SetActive(false);
        contractsPanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void OpenShop()
    {
        hqPanel.SetActive(false);
        contractsPanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    // Update top bar data from HQManager
    private void UpdateTopBar()
    {
        if (HQManager.Instance != null)
        {
            if (moneyText != null) moneyText.text = $"Money: ${HQManager.Instance.currentMoney}";
            if (dayText != null) dayText.text = $"Day: {HQManager.Instance.currentDay}";
            if (reputationText != null) reputationText.text = $"Rep: {HQManager.Instance.companyReputation}";
        }
    }
}