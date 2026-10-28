using UnityEngine;

[CreateAssetMenu(fileName = "NewObjective", menuName = "Ghost Tycoon/Objective Data")]
public class ObjectiveData : ScriptableObject
{
    public string objectiveTitle;     // goal of mission
    [TextArea(2, 4)]
    public string objectiveDetails;   // descrtibtion of mission
    public int flatObjectiveReward = 150; // Fixed reward per goal ($)
}