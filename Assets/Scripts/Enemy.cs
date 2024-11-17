using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float _speed;
    Coroutine _MoveCoroutine;
    public void SetPath(Node firstNode, List<Node> path)
    {
        if (_MoveCoroutine != null)
            StopCoroutine(_MoveCoroutine);

        transform.position = firstNode.transform.position;
        _MoveCoroutine = StartCoroutine(Move(path));
    }

    IEnumerator Move(List<Node> path)
    {
        while (path.Count > 0) 
        {
            var dir = path[0].transform.position - transform.position;

            transform.position += dir.normalized * _speed * Time.deltaTime;

            if (dir.magnitude <= 0.2f)
                path.RemoveAt(0);

            yield return null;
        }

        _MoveCoroutine = null;

    }
}
