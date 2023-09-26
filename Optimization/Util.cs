using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public const string objectFormat = @"------ [{0}]";
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if(component == null)
        {
            component = go.AddComponent<T>();
        }

        return component as T;
    }

    public static T Find<T> (this GameObject go, string name, bool recursive = false) where T : UnityEngine.Object
    {
        T component = null;

        if(typeof(T) == typeof(GameObject))
        {
            return GameObject.Find(name) as T;
        }
        else
        {
            foreach (T t in go.transform.GetComponentsInChildren<T>(recursive))
            {
                if (t.name.Equals(name))
                {
                    component = t;

                    return component;
                }
            }
        }        

        return component;
    }    


    public static GameObject Instantiate(GameObject go) 
    {
        Poolable poolable = go.GetComponent<Poolable>();
        GameObject instance;
        if (poolable == null)
        {
            instance = GameObject.Instantiate(go);            
        }
        else
        {
            instance = PoolManager.Pool.Get(go);
        }

        return instance;
    }

    public static void Destroy(GameObject go)
    {
        Poolable poolable = go.GetComponent<Poolable>();        
        if (poolable == null)
        {
            GameObject.Destroy(go);
        }
        else
        {
            PoolManager.Pool.Destroy(go);
        }
    }

    public static void SetActivePlayOnEnable(GameObject go)
    {
        go.SetActive(false);
        go.SetActive(true);
    }
}
