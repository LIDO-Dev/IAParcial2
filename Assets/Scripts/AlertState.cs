using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : NPCState
{
    private Vector3 lastKnownPosition;

    public AlertState(NPC npc, Vector3 position) : base(npc)
    {
        this.lastKnownPosition = position;
    }
    public void SetAlertPosition(Vector3 position)
    {
        this.lastKnownPosition = position;
    }

    public override void Enter()
    {
        npc.SetDestination(lastKnownPosition);
        //Debug.Log($"{npc.name} entrando en estado de alerta hacia {lastKnownPosition}.");
        //Debug.Log($"NPC {npc.gameObject.name} entrando en estado de alerta hacia {lastKnownPosition}");
    }

    public override void Update()
    {

        if (npc.CanSeePlayer())
        {
            npc.lastKnowPlayerPosition = npc.player.position;
            npc.SetState(new ChasingState(npc));
        }

        if (npc.IsAtDestination())
        {
            npc.SetState(new PatrollingState(npc));
            //Debug.Log($"{npc.name} llegó al destino de alerta en {lastKnownPosition}. Regresando a patrullaje.");
        }
        else
        {
            //Debug.Log($"{npc.name} está en camino hacia {lastKnownPosition}. Quedan asdf");
        }
    }


    public override void Exit()
    {
        //Debug.Log($"NPC {npc.gameObject.name} saliendo del estado de alerta.");
    }
}
