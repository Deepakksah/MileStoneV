using System;

namespace VideoOS.Installer.Configuration;

[Serializable]
public class KeyValueParameters<KeyType, ValueType>
{
	private KeyType _key;

	private ValueType _value;

	public KeyType Key
	{
		get
		{
			return _key;
		}
		set
		{
			_key = value;
		}
	}

	public ValueType Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}
}
