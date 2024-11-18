using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    public PatrollingState PatrollingState { get; private set; }
    public AlertState AlertState { get; private set; }
    public ChasingState ChasingState { get; private set; }
    public NPCManager NPCManager { get; private set; }
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
        PatrollingState = new PatrollingState(this);
        AlertState = new AlertState(this, Vector3.zero);
        ChasingState = new ChasingState(this);
        NPCManager = new NPCManager();
        SetState(PatrollingState);
        currentNode = Aestrella.GetNearestNode(this.transform.position);
        //NPCManager.Instance.RegisterNPC(this);
    }

    void Update()
    {
        currentState.Update();
        //currentNode = Aestrella.GetNearestNode(this.transform.position);
       
        if (CanSeePlayer())
        {
            lastKnowPlayerPosition = player.position;
            AlertOthers();
            SetState(new ChasingState(this));
        }


        if (currentPath != null && currentNodeIndex < currentPath.Count)
        {
            Node targetNode = currentPath[currentNodeIndex];
            Vector3 targetPosition = targetNode.transform.position;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                //Debug.Log($"{name} alcanzó el nodo {targetNode.name}. Avanzando al siguiente.");
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
        Node targetNode = Aestrella.GetNearestNode(position);
        if (targetNode != null)
        {
            currentPath = Aestrella.FindPath(currentNode, targetNode);
            if (currentPath != null && currentPath.Count > 0)
            {
                //Debug.Log($"Camino encontrado para {name} hacia {position}. Nodos en el camino: {currentPath.Count}");
                currentNodeIndex = 0;
            }
            else
            {
                //Debug.LogWarning($"No se encontró camino para {name} hacia {position}.");
            }
        }
        else
        {
            //Debug.LogWarning($"No se encontró un nodo cercano para la posición {position}.");
        }
    }

    public bool IsAtDestination()
    {
        return currentPath == null || currentNodeIndex >= currentPath.Count;
    }

    public void AlertOthers()
    {

        foreach (NPC npc in NPCManager.Instance.AllNPCs)
        {
            Vector3 playerPosition = lastKnowPlayerPosition;

            if (npc == this) continue;
            Debug.Log($"Alertando a {npc.name}");
            if (npc.AlertState != null)
            {
                npc.AlertState.SetAlertPosition(lastKnowPlayerPosition);
                npc.SetState(npc.AlertState);
            }
            else
            {
                Debug.LogWarning($"El NPC {npc.name} no tiene un AlertState configurado.");
            }
        }
    }

}
