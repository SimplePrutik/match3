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

	/// <summary>
	/// Get config data
	/// </summary>
	/// <param name="field"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetConfig<T>(string field)
	{
		return _data.ContainsKey(field) ? (T)_data[field] : default(T);
	}

	/// <summary>
	/// Set config data and send it to syncing listeners
	/// </summary>
	/// <param name="field"></param>
	/// <param name="value"></param>
	/// <typeparam name="T"></typeparam>
	public void SetConfig<T>(string field, T value)
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
