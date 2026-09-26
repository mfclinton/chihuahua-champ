using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    #region Fields
    [SerializeField]
    private List<TKey> keys = new List<TKey>(); // List to store keys
    [SerializeField]
    private List<TValue> values = new List<TValue>(); // List to store values
    #endregion

    #region Properties
    public int Count { get { return keys.Count; } } // Number of key-value pairs in the dictionary
    #endregion

    #region Public Methods
    // Add a key-value pair to the dictionary
    public void Add(TKey key, TValue value)
    {
        if (key == null)
            throw new ArgumentNullException("key");

        int index = keys.IndexOf(key);
        if (index == -1)
        {
            keys.Add(key);
            values.Add(value);
        }
        else
        {
            values[index] = value;
        }
    }

    // Check if the dictionary contains a specific key
    public bool ContainsKey(TKey key)
    {
        return keys.Contains(key);
    }

    // Remove a key-value pair from the dictionary
    public bool Remove(TKey key)
    {
        if (key == null)
            throw new ArgumentNullException("key");

        int index = keys.IndexOf(key);
        if (index != -1)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
            return true;
        }
        return false;
    }

    // Try to get the value associated with a key
    public bool TryGetValue(TKey key, out TValue value)
    {
        int index = keys.IndexOf(key);
        if (index != -1)
        {
            value = values[index];
            return true;
        }
        value = default(TValue);
        return false;
    }

    // Clear all key-value pairs from the dictionary
    public void Clear()
    {
        keys.Clear();
        values.Clear();
    }

    // Get the key at a specific index in the dictionary
    public TKey GetKey(int index)
    {
        return keys[index];
    }

    // Get the value at a specific index in the dictionary
    public TValue GetValue(int index)
    {
        return values[index];
    }

    // Set the value at a specific index in the dictionary
    public void SetValue(int index, TValue value)
    {
        values[index] = value;
    }
    #endregion
}
