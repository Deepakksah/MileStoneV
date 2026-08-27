using System;
using System.Collections.Generic;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class SerializationContext
{
	private readonly Dictionary<string, object> _hostObjects = new Dictionary<string, object>();

	public AutomationObjectSerializationOptions Options { get; }

	public SerializationContext(AutomationObjectSerializationOptions options)
	{
		Options = options;
	}

	public THostObject GetOrAddHostObject<THostObject>(string keyPrefix, object obj, Func<THostObject> factory)
	{
		string key = GetKey(keyPrefix, obj);
		if (!_hostObjects.TryGetValue(key, out var value))
		{
			value = factory();
			_hostObjects.Add(key, value);
		}
		return (THostObject)value;
	}

	private static string GetKey(string keyPrefix, object obj)
	{
		return FormattableString.Invariant($"{keyPrefix}{obj.GetHashCode()}");
	}
}
