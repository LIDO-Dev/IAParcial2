using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aestrella
{
    public static List<Node> FindPath(Node start, Node goal)
    {
        if(start == null || goal == null) 
        {
            Debug.Log("Goal or StartNode is Null");
            return null;
        }
        var openSet = new List<Node> { start };
        var cameFrom = new Dictionary<Node, Node>();
        var gScore = new Dictionary<Node, float>();
        var fScore = new Dictionary<Node, float>();

        foreach (var node in GameObject.FindObjectsOfType<Node>())
        {
            if(node == null) continue;
            gScore[node] = float.MaxValue;
            fScore[node] = float.MaxValue;
        }

        gScore[start] = 0;
        fScore[start] = Heuristic(start, goal);

        while (openSet.Count > 0)
        {
            var current = GetLowestFScore(openSet, fScore);
            if (current == goal)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            foreach (var neighbor in current.neighbors)
            {
                float tentativeGScore = gScore[current] + current.GetCost(neighbor);

                if (tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null;
    }
    private static Node GetLowestFScore(List<Node> openSet, Dictionary<Node, float> fScore)
    {
        Node lowest = null;
        float minScore = float.MaxValue;

        foreach (var node in openSet)
        {
            if (fScore[node] < minScore)
            {
                minScore = fScore[node];
                lowest = node;
            }
        }

        return lowest;
    }

    private static float Heuristic(Node a, Node b)
    {
        return Vector3.Distance(a.Position, b.Position);
    }

    private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
    {
        var path = new List<Node> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    public static Node GetNearestNode(Vector3 position)
    {
        Node nearestNode = null;
        float nearestDistance = float.MaxValue;

        foreach (var node in GameObject.FindObjectsOfType<Node>())
        {
            float distance = Vector3.Distance(position, node.Position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }
}
