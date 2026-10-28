using UnityEngine;

[CreateAssetMenu(fileName = "NewLocation", menuName = "Ghost Tycoon/Location Data")]
public class LocationData : ScriptableObject
{
    public string locationName = "New Location";
    
    [Header("Difficulty")]
    [Tooltip("Base difficulty score of this location")]
    public int difficultyScore = 10;
}