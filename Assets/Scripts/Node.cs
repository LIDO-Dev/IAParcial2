using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Node : MonoBehaviour
{
    public TextMeshProUGUI textCost;
    [SerializeField]List<Node> _neighbors = new List<Node>();
    public bool Block { get; private set; }
    public int Cost = 1;

    Grid _grid;
    int _x;
    int _y;

    public void Initialize(Grid grid, int x, int y)
    {
        _grid = grid;
        _x = x;
        _y = y;

        textCost.text = Cost.ToString();
    }
    public List<Node> GetNeighbors
    {
        get 
        {
            if (_neighbors.Count > 0)
                return _neighbors;

            var nodeRight = _grid.GetNode(_x + 1, _y);
            if(nodeRight != null)
                _neighbors.Add(nodeRight);

            var nodeLeft = _grid.GetNode(_x - 1, _y);
            if (nodeLeft != null)
                _neighbors.Add(nodeLeft);

            var nodeUp = _grid.GetNode(_x, _y + 1);
            if (nodeUp != null)
                _neighbors.Add(nodeUp);

            var nodeDown = _grid.GetNode(_x, _y - 1);
            if (nodeDown != null)
                _neighbors.Add(nodeDown);

            return _neighbors;
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameManager.instance.SetStartingNode(this);
            Block = false;
            gameObject.layer = 0;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameManager.instance.SetGoalNode(this);
            Block = false;
            gameObject.layer = 0;
        }

        if (Input.GetMouseButtonDown(2))
        {
            Block = !Block;

            if(Block)
                GetComponent<MeshRenderer>().material.color = Color.gray;
            else
                GetComponent<MeshRenderer>().material.color = Color.white;

            gameObject.layer = Block ? 6 : 0;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Cost++;
            if (Cost > 50)
                Cost = 50;
                
            textCost.text = Cost.ToString();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Cost--;
            if (Cost < 1)
                Cost = 1;

            textCost.text = Cost.ToString();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var node in _neighbors)
        {
            Gizmos.DrawLine(transform.position, node.transform.position);
        }
    }
}
