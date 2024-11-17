using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState : NPCState
{
    private int currentWaypointIndex = 0;

    public PatrollingState(NPC npc) : base(npc) { }

    public override void Enter()
    {
        npc.SetDestination(npc.waypoints[currentWaypointIndex].Position);
    }

    public override void Update()
    {
        if (npc.IsAtDestination())
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % npc.waypoints.Length;
            npc.SetDestination(npc.waypoints[currentWaypointIndex].Position);
        }

        if (npc.CanSeePlayer())
        {
            npc.SetState(new ChasingState(npc));
        }
    }

    public override void Exit() { }
}
