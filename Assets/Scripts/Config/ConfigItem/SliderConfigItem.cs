using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderConfigItem : ConfigItem
{
    protected Slider Slider => GetComponent<Slider>();

    public Text text_value;
    
    public override void SetConfigValue()
    {
        GameConfiguration.Config().SetConfig(tag, (int)Slider.value);
        text_value.text = Slider.value.ToString();
    }
    
}