using System.Collections.Generic;

namespace VideoOS.Mobile.Management;

public class PropertyChecker
{
	public delegate void OnValue(string value);

	public delegate bool OnBool(bool value);

	public delegate void OnMissing();

	private readonly Dictionary<string, string> _properties;

	public PropertyChecker(Dictionary<string, string> properties)
	{
		_properties = properties;
	}

	public void Check(string name, OnValue onValue, OnMissing onMissing = null)
	{
		if (_properties.ContainsKey(name))
		{
			onValue?.Invoke(_properties[name]);
		}
		else
		{
			onMissing?.Invoke();
		}
	}

	public void Check(string name, OnBool onBool, OnMissing onMissing = null)
	{
		Check(name, delegate(string value)
		{
			onBool?.Invoke(value == "True");
		}, onMissing);
	}
}
