using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingState : FSM
{
    public ChasingState(NPC npc) : base(npc) { }

    public override void Enter()
    {
        npc.AlertOthers();
    }

    public override void Update()
    {
        if (npc.CanSeePlayer())
        {
            npc.SetDestination(npc.player.position);
        }
        else
        {
            npc.SetState(new PatrollingState(npc));
        }
    }

    public override void Exit() { }
}
