using UnityEngine;

public class HQTabController : MonoBehaviour
{
    [Header("Tabs")]
    public GameObject hiredStaffTab;
    public GameObject recruitmentTab;

    public void OpenHiredStaffTab()
    {
        if (hiredStaffTab != null) hiredStaffTab.SetActive(true);
        if (recruitmentTab != null) recruitmentTab.SetActive(false);
    }

    public void OpenRecruitmentTab()
    {
        if (hiredStaffTab != null) hiredStaffTab.SetActive(false);
        if (recruitmentTab != null) recruitmentTab.SetActive(true);
    }
}