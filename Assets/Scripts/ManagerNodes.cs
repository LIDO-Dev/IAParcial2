using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerNodes : MonoBehaviour
{
    public static ManagerNodes instance;
    Node[] _totalNodes;
    void Start()
    {
        instance = this;
        _totalNodes = FindObjectsOfType<Node>();    
    }

    //El enemigo podria hacer = 
    //_pathfinding.CalculateAStar(ManagerNodes.instance.GetMinNode(transform.position), 
    //                              ManagerNodes.instance.GetMinNode(player.position));
    public Node GetMinNode(Vector3 position)
    {
        Node minNode = null;
        float minDistance = Mathf.Infinity;

        foreach (var node in _totalNodes)
        {
            float dist = (node.transform.position - position).magnitude;

            if (dist < minDistance)
            {
                if(GameManager.instance.InLineOfSight(node.transform.position, position))
                {
                    minNode = node;
                    minDistance = dist;
                }
            }
        }

        return minNode;
    }
}
