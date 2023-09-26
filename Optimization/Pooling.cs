using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling
{
    public Stack<GameObject> _stack = new Stack<GameObject>();
    public GameObject _original;
    public Transform _root;

    public Pooling (GameObject original)
    {
        _original = original;
        GameObject go = new GameObject() { name = original.name + "Pool" };
        go.transform.parent = PoolManager.Pool._root;
        _root = go.transform;
    }

    public void MadePool (int count = 5)
    {        
        for(int i = 0; i <count; i++)
        {
            GameObject go = GameObject.Instantiate(_original);
            go.name = _original.name;
            go.transform.SetParent(_root);
            go.SetActive(false);
            _stack.Push(go);
        }
    }

    public GameObject Get ()
    {
        if (_stack.Count == 0)
            MadePool();

        return _stack.Pop();
    }

    public void Destroy(GameObject go)
    {
        _stack.Push(go);
    }

}
