using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfiguration : MonoBehaviour {

	Dictionary<string, int> config = new Dictionary<string, int>{{"width", 5}, {"height", 5}};
	void ValueChangeHandler(string field, int value)
	{
		if (!config.ContainsKey(field))
			return;
		config[field] = value;
	}
	private void OnEnable()
	{
		ValueField.OnValueChanged += ValueChangeHandler;
	}

	private void OnDisable()
	{
		ValueField.OnValueChanged -= ValueChangeHandler;
	}
}
