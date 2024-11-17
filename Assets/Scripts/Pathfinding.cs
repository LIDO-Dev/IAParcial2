using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<Node> CalculateBFS(Node startingNode, Node goalNode)
    {
        var frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);
        
        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if(current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }
            foreach (var next in current.GetNeighbors)
            {
                if (!next.Block && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> CalculateDijkstra(Node startingNode, Node goalNode)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }
            foreach (var next in current.GetNeighbors)
            {
                if (next.Block) continue;

                int newCost = costSoFar[current] + next.Cost;

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, newCost);
                    cameFrom.Add(next, current);
                }else if (costSoFar[next] > newCost)
                {
                    frontier.Enqueue(next, newCost);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> CalculateGreedyBFS(Node startingNode, Node goalNode)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }
            foreach (var next in current.GetNeighbors)
            {
                if (next.Block) continue;
  
                if (!cameFrom.ContainsKey(next))
                {
                    float priority = Vector3.Distance(next.transform.position, goalNode.transform.position);
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> CalculateAStar(Node startingNode, Node goalNode)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }
                path.Reverse();
                return path;
            }
            foreach (var next in current.GetNeighbors)
            {
                if (next.Block) continue;

                int newCost = costSoFar[current] + next.Cost;
                float priority = newCost + Vector3.Distance(next.transform.position, goalNode.transform.position);

                if (!costSoFar.ContainsKey(next))
                {
                    costSoFar.Add(next, newCost);
                    frontier.Enqueue(next, priority);
                    cameFrom.Add(next, current);
                }
                else if (costSoFar[next] > newCost)
                {
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                    costSoFar[next] = newCost;
                }
            }
        }

        return new List<Node>();
    }

    public List<Node> CalculateTheta(Node startingNode, Node goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (GameManager.instance.InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position))
                listNode.RemoveAt(current + 1);
            else
                current++;
        }

        return listNode;
    }

    public IEnumerator CoroutineBFS(Node startingNode, Node goalNode)
    {
        var frontier = new Queue<Node>();
        frontier.Enqueue(startingNode);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    current.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    yield return new WaitForSeconds(0.05f);
                }
                path.Reverse();
                yield return null;
            }
            foreach (var next in current.GetNeighbors)
            {
                if (!next.Block && !cameFrom.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    cameFrom.Add(next, current);
                    next.GetComponent<MeshRenderer>().material.color = Color.blue;
                }

                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public IEnumerator CoroutineDijkstra(Node startingNode, Node goalNode)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    current.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    yield return new WaitForSeconds(0.03f);
                }
                path.Reverse();
                yield return null;
            }
            else
            {
                current.GetComponent<MeshRenderer>().material.color = Color.cyan;

                foreach (var next in current.GetNeighbors)
                {
                    if (next.Block) continue;

                    int newCost = costSoFar[current] + next.Cost;
                    
                    if (!costSoFar.ContainsKey(next))
                    {
                        costSoFar.Add(next, newCost);
                        frontier.Enqueue(next, newCost);
                        next.GetComponent<MeshRenderer>().material.color = Color.blue;
                        cameFrom.Add(next, current);
                    }
                    else if (costSoFar[next] > newCost)
                    {
                        frontier.Enqueue(next, newCost);
                        cameFrom[next] = current;
                        costSoFar[next] = newCost;
                    }

                    yield return new WaitForSeconds(0.03f);
                }
            }
        }
    }

    public IEnumerator CoroutineGreedyBFS(Node startingNode, Node goalNode)
    {
        var frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    current.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    yield return new WaitForSeconds(0.03f);
                }
                path.Reverse();
                yield return null;
            }
            else
            {
                current.GetComponent<MeshRenderer>().material.color = Color.cyan;

                foreach (var next in current.GetNeighbors)
                {
                    if (next.Block) continue;

                    if (!cameFrom.ContainsKey(next))
                    {
                        int priority = (int)Vector3.Distance(next.transform.position, goalNode.transform.position);
                        frontier.Enqueue(next, priority);
                        next.GetComponent<MeshRenderer>().material.color = Color.blue;
                        cameFrom.Add(next, current);
                    }

                    yield return new WaitForSeconds(0.03f);
                }
            }

        }

    }

}
