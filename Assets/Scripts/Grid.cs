using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node prefab;
    public int width, height;
    public float offset;

    Node[,] _grid;

    private void Start()
    {
        _grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var pos = new Vector2(x, y ) * offset;
                var go = Instantiate(prefab);
                go.transform.position = pos;               
                _grid[x, y] = go;
                go.Initialize(this, x, y);
            }
        }
    }

    public Node GetNode(int x, int y)
    {
        if(x < 0 || x >= width || y < 0 || y >= height)
            return null;

        return _grid[x, y];
    }
}
