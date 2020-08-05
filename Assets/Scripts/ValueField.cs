
using System;
using UnityEngine;
using UnityEngine.UI;

public class ValueField : MonoBehaviour
{
    public static event Action<string, int> OnValueChanged = delegate(string s, int i) {  };

    public void ValueChanged()
    {
        var value = GetValue();
        OnValueChanged(tag, value);
    }

    private int GetValue()
    {
        if (GetComponent<InputField>() != null)
            return Convert.ToInt32(GetComponent<InputField>().text);
        
        if (GetComponent<Slider>() != null)
            return (int)GetComponent<Slider>().value;

        return 0;
    }

    private void SetValue(string _name, int value)
    {
        if (!CompareTag(_name))
            return;
        if (GetComponent<InputField>() != null)
            GetComponent<InputField>().text = value.ToString();

        if (GetComponent<Slider>() != null)
            GetComponent<Slider>().value = value;
    }
    
    private void OnEnable()
    {
        OnValueChanged += SetValue;
    }

    private void OnDisable()
    {
        OnValueChanged -= SetValue;
    }
}