using System;
using System.Globalization;
using System.Resources;

namespace VideoOS.Utilities.Telemetry;

public class LogItem
{
	private readonly string _field;

	private readonly string _value;

	public string Field => _field;

	public string Value => _value;

	public LogItem(string field, string value)
	{
		_field = field;
		_value = value;
	}

	public LogItem(string field, Guid value)
	{
		_field = field;
		_value = value.ToString();
	}

	public LogItem(string field, DateTime value)
	{
		_field = field;
		_value = value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
	}

	public LogItem(string field, string value, params object[] args)
	{
		_field = field;
		_value = string.Format(CultureInfo.CurrentCulture, value, args);
	}

	public LogItem(string field, Enum value, ResourceManager resourceManager)
	{
		_field = field;
		string text = value.ToString();
		string name = value.GetType().Name;
		_value = resourceManager.GetString("Enum_" + name + "_" + text, CultureInfo.CurrentCulture);
	}
}
