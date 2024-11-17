using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public list<Node> vecinos;
    public Vector3 Position => transform.position;
    public float getCost (Node vecino)
    {
        return Vector3.distance(Position, vecino.Position);
    } 

}
