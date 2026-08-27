using System;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class ObjectValueInfo
{
	public string Name { get; }

	public object Value { get; }

	public Type Type { get; }

	public ObjectValueInfo(string name, object value, Type type)
	{
		Name = name;
		Value = value;
		Type = type;
	}
}
