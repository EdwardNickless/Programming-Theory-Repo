using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField]
    private List<TKey> keys;
    [SerializeField]
    private List<TValue> values;

    public SerializableDictionary()
    {
        keys = new List<TKey>();
        values = new List<TValue>();
    }

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }

    public int Count
    {
        get { return keys.Count; }
    }

    public TKey GetKey(int index)
    {
        return keys[index];
    }

    public TValue GetValue(int index)
    {
        return values[index];
    }
}
