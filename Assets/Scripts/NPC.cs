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

    public float moveSpeed = 5f;

    public float alertRadius = 10f;

    public Vector3 lastKnowPlayerPosition;

    private Node currentNode;

    private List<Node> currentPath;

    private int currentNodeIndex;

    private NPCState currentState;

    void Start()
    {
        SetState(new PatrollingState(this));
    }

    void Update()
    {
        currentState.Update();

        if (currentPath != null && currentNodeIndex < currentPath.Count)
        {
            Node targetNode = currentPath[currentNodeIndex];
            Vector3 targetPosition = targetNode.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                currentNode = targetNode;
                currentNodeIndex++;
            }
        }
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
        Node targetNode = Graph.GetNearestNode(position);
        if (targetNode != null)
        {
            currentPath = Aestrella.FindPath(currentNode, targetNode);
            if (currentPath != null && currentPath.Count > 0)
            {
                currentNodeIndex = 0;
            }
        }
    }

    public bool IsAtDestination()
    {
        return currentPath == null || currentNodeIndex >= currentPath.Count;
    }

    public void AlertOthers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, alertRadius);
        foreach (Collider collider in colliders)
        {
            NPC otherNPC = collider.GetComponent<NPC>();
            if (otherNPC != null && otherNPC != this)
            {
                otherNPC.SetDestination(lastKnowPlayerPosition);
            }
        }
    }
}
