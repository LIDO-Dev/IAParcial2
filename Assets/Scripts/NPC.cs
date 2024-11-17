using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Node[] waypoints;
    public Transform player;
    public float viewDistance;
    public float viewAngle;
    public LayerMask obstacleMask;

    private NPCState currentState;

    void Start()
    {
        SetState(new PatrollingState(this));
    }

    void Update()
    {
        currentState.Update();
    }

    public void SetState(NPCState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (angle < viewAngle / 2 && Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            if (!Physics.Raycast(transform.position, directionToPlayer, Vector3.Distance(transform.position, player.position), obstacleMask))
            {
                return true;
            }
        }

        return false;
    }

    public void SetDestination(Vector3 position)
    {
        // Configurar movimiento al destino
    }

    public bool IsAtDestination()
    {
        // Comprobar si ha llegado al destino
        return true; // Placeholder
    }

    public void AlertOthers()
    {
        // Notificar a otros NPCs
    }
}
