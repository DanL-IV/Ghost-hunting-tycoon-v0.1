using UnityEngine;

public enum GhostType
{
    Poltergeist,
    Banshee,
    Oni,
    Demon,
    Shade
}

[CreateAssetMenu(fileName = "NewGhost", menuName = "Ghost Tycoon/Ghost Data")]
public class GhostData : ScriptableObject
{
    public string ghostName = "Unknown Entity";
    public GhostType ghostType;

    [Header("Ghost Mechanics")]
    public int baseGhostBounty = 100;
    
    [Range(0, 100)]
    public int baseDifficulty = 30;

    [Range(0, 100)]
    public float aggressionLevel = 20f;

    [Header("Special Behavioral Rules")]
    [Tooltip("If true, the ghost hides when personnel count exceeds maximum limit")]
    public bool hasPersonnelLimit;

    [Tooltip("Max allowed personnel count (e.g. Shade = 1)")]
    public int maxAllowedPersonnel;
}