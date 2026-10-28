using UnityEngine;

[CreateAssetMenu(fileName = "NewEmployee", menuName = "Ghost Tycoon/Employee Data")]
public class EmployeeData : ScriptableObject
{
    public string employeeName;
    public string role; // Investigator, Exorcist, Influencer, etc.
    public float hireCost;
    public float dailySalary;

    [Header("Skills")]
    public float investigationSkill;
    public float exorcismSkill;
    public float recordingSkill;

    [Header("Stress & Status")]
    [Range(0f, 100f)] public float stressLevel = 0f; // From 0 to 100%
}