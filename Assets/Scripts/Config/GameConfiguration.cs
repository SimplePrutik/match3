using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameConfiguration
{

	private static GameConfiguration _config;
	public static GameConfiguration Config() => _config ?? (_config = new GameConfiguration());

	private readonly Dictionary<string, object> _data = new Dictionary<string, object>
	{ };
	
	public Action<string, object> OnConfigChanged = delegate(string field, object value) {  };

	public object GetConfig(string field)
	{
		return _data.ContainsKey(field) ? _data[field] : null;
	}

	public void SetConfig(string field, object value)
	{
		OnConfigChanged(field, value);
		if (_data.ContainsKey(field))
		{
			_data[field] = value;
			return;
		}
		_data.Add(field, value);
	}
}
