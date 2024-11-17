using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : NPCState
{
    private Vector3 lastKnownPosition;

    public AlertState(NPC npc, Vector3 position) : base(npc)
    {
        lastKnownPosition = position;
    }

    public override void Enter()
    {
        npc.SetDestination(lastKnownPosition);
    }

    public override void Update()
    {
        if (npc.IsAtDestination())
        {
            npc.SetState(new PatrollingState(npc));
        }
    }

    public override void Exit() { }
}
