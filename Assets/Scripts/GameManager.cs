using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Pathfinding pathfinding;
    public Enemy enemy;
    Node _startingNode;
    Node _goalNode;
    public LayerMask mask;
    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_startingNode == null || _goalNode == null) return;

            //StartCoroutine(pathfinding.CoroutineGreedyBFS(_startingNode, _goalNode));
            enemy.SetPath(_startingNode, pathfinding.CalculateTheta(_startingNode, _goalNode));
        }
    }

    public void SetStartingNode(Node node)
    {
        if(_startingNode != null)
            _startingNode.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        _startingNode = node;
        node.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    public void SetGoalNode(Node node)
    {
        if (_goalNode != null)
            _goalNode.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        _goalNode = node;
        node.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;
        return !Physics.Raycast(start, dir, dir.magnitude, mask);
    }
}
