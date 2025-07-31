using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> serializedKeys = new List<TKey>();
    [SerializeField] private List<TValue> serializedValues = new List<TValue>();

    public void OnBeforeSerialize()
    {
        serializedKeys.Clear();
        serializedValues.Clear();
        foreach (var pair in this)
        {
            serializedKeys.Add(pair.Key);
            serializedValues.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (serializedKeys.Count != serializedValues.Count)
        {
            throw new Exception("Serialized keys and values are not the same count");
        }
        for (int i = 0; i < serializedKeys.Count; i++)
        {
            this[serializedKeys[i]] = serializedValues[i];
        }
    }
}

