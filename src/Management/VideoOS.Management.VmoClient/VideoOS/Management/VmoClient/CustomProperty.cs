namespace VideoOS.Management.VmoClient;

public class CustomProperty
{
	private string _value;

	public string Key { get; private set; }

	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
			Dirty = true;
		}
	}

	public bool Dirty { get; private set; }

	public CustomProperty(string key, string value)
		: this(key, value, dirty: false)
	{
		Key = key;
		_value = value;
	}

	public CustomProperty(string key, string value, bool dirty)
	{
		Key = key;
		_value = value;
		Dirty = dirty;
	}
}
