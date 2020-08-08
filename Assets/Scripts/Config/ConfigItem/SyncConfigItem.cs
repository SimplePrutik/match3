
using System;
using UnityEngine;

public class SyncConfigItem : MonoBehaviour, IConfigItem
{
    private void Awake()
    {
        SetConfigValue();
    }

    public virtual void SetConfigValue()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Synchronise data between different UI elements linked to the same value
    /// </summary>
    /// <param name="field"></param>
    /// <param name="value"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void SyncValue(string field, object value)
    {
        throw new System.NotImplementedException();
    }
}