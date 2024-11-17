using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors;
    public Vector3 Position => transform.position;
    public float GetCost (Node neighbor)
    {
        return Vector3.Distance(Position, neighbor.Position);
    } 

}
