using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager _instance;
    public static PoolManager Pool { get {return _instance; } }

    Dictionary<string, Pooling> _dic = new Dictionary<string, Pooling>();

    public Transform _root;
    public Transform _rootField;

    public Dictionary<string, Pooling> PoolDictionary { get { return _dic; } }

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Pool");

            if (go == null)
                go = new GameObject() { name = "@Pool" };

            _instance = go.GetOrAddComponent<PoolManager>();
            DontDestroyOnLoad(go);
        }
        else
            Destroy(this.gameObject);

        if (_root == null)
        {
            GameObject root = GameObject.Find(string.Format(Util.objectFormat, "Pool"));

            if(root==null)
                _root = new GameObject() { name = string.Format(Util.objectFormat,"Pool") }.transform;
        }

        _rootField = Camera.main.transform.parent;
    }

    public GameObject Get(GameObject poolObject) 
    {
        if(_dic.ContainsKey(poolObject.name) == false)
        {
            _dic.Add(poolObject.name, new Pooling(poolObject));
        }
        GameObject instance = _dic[poolObject.name].Get();
        instance.transform.SetParent(_rootField);
        instance.SetActive(true);
        return instance;
    }
    public void Destroy(GameObject poolObject) 
    {
        if (_dic.ContainsKey(poolObject.name) == false)
        {
            _dic.Add(poolObject.name, new Pooling(poolObject));
        }

        poolObject.SetActive(false);
        poolObject.transform.SetParent(_dic[poolObject.name]._root);
        _dic[poolObject.name].Destroy(poolObject);
    }

    public void Refresh()
    {
        _root = null;
        _rootField = null;
        _dic.Clear();
    }

}
