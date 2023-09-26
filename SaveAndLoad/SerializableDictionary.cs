using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey,TValue> : Dictionary<TKey,TValue>,ISerializationCallbackReceiver 
{
    [SerializeField]
    List<TKey> _keyList = new List<TKey>();
    [SerializeField]
    List<TValue> _valueList = new List<TValue>();    

    public void OnBeforeSerialize()
    {
        

        _keyList.Clear();
        _valueList.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            _keyList.Add(pair.Key);
            _valueList.Add(pair.Value);
        }
    }
    public void OnAfterDeserialize()
    {
        this.Clear();

        if (_keyList.Count != _valueList.Count)
            Debug.Log("Falid DeSerialize not same key,value Count : SerializableDictionary.cs");

        for (int i = 0; i < _keyList.Count; i++)
        {
            this.Add(_keyList[i], _valueList[i]);
        }
    }
}
