using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : NPCState
{
    public ChasingState(NPC npc) : base(npc) { }

    public override void Enter()
    {
        npc.SetDestination(npc.player.transform.position);
        //Debug.Log($"NPC {npc.gameObject.name} está persiguiendo al jugador.");
    }

    public override void Update()
    {
        if (npc.CanSeePlayer())
        {
            npc.lastKnowPlayerPosition = npc.player.position;
            npc.SetDestination(npc.player.transform.position);
        }

        if (npc.IsAtDestination())
        {
            npc.SetState(new PatrollingState(npc));
        }
    }

    public override void Exit()
    {
        //Debug.Log($"NPC {npc.gameObject.name} dejó de perseguir al jugador.");
    }
}
