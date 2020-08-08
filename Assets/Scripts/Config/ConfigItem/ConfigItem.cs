
using System;
using UnityEngine;

public class ConfigItem : MonoBehaviour, IConfigItem
{
    private void Awake()
    {
        SetConfigValue();
    }

    public virtual void SetConfigValue()
    {
        throw new System.NotImplementedException();
    }

}