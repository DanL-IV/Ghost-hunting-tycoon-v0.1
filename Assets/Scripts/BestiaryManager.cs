using UnityEngine;
using System.Collections.Generic;

public class BestiaryManager : MonoBehaviour
{
    public static BestiaryManager Instance { get; private set; }

    [Header("Bestiary Tracking")]
    public List<string> discoveredGhostTypes = new List<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DiscoverGhost(string ghostTypeName)
    {
        if (!discoveredGhostTypes.Contains(ghostTypeName))
        {
            discoveredGhostTypes.Add(ghostTypeName);
            Debug.Log($"[Bestiary] New ghost entry unlocked in encyclopedia: {ghostTypeName}!");
        }
    }

    public bool IsDiscovered(string ghostTypeName)
    {
        return discoveredGhostTypes.Contains(ghostTypeName);
    }
}