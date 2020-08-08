using System;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxSyncConfigItem : SyncConfigItem
{
    protected InputField Input => GetComponent<InputField>();
    
    public override void SetConfigValue()
    {
        GameConfiguration.Config().SetConfig(tag, Convert.ToInt32(Input.text));
    }

    protected override void SyncValue(string field, object value)
    {
        if (CompareTag(field))
            Input.text = value.ToString();
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