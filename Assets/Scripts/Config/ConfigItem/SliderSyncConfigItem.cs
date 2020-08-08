using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderSyncConfigItem : SyncConfigItem
{
    protected Slider Slider => GetComponent<Slider>();
    
    public override void SetConfigValue()
    {
        GameConfiguration.Config().SetConfig(tag, (int)Slider.value);
    }

    protected override void SyncValue(string field, object value)
    {
        if (CompareTag(field))
            Slider.value = Convert.ToInt32(value);
    }

    private void OnEnable()
    {
        GameConfiguration.Config().OnConfigChanged += SyncValue;
    }
    
    private void OnDisable()
    {
        GameConfiguration.Config().OnConfigChanged -= SyncValue;
    }
}