using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance;
    public List<NPC> AllNPCs = new List<NPC>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterNPC(NPC npc)
    {
        if (!AllNPCs.Contains(npc))
        {
            AllNPCs.Add(npc);
        }
    }

    public void UnregisterNPC(NPC npc)
    {
        if (AllNPCs.Contains(npc))
        {
            AllNPCs.Remove(npc);
        }
    }
}
