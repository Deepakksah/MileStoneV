using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace VideoOS.Management.Server;

public class TaskPropertyCollection : Collection<KeyValue>
{
	public bool TryGetValue(string propertyName, out string value)
	{
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		KeyValue keyValue = this.FirstOrDefault((KeyValue p) => string.Equals(p.Key, propertyName, StringComparison.Ordinal));
		if (keyValue == null)
		{
			value = null;
			return false;
		}
		value = keyValue.Value;
		return true;
	}

	public string GetValue(string propertyName)
	{
		if (propertyName == null)
		{
			throw new ArgumentNullException("propertyName");
		}
		return (this.FirstOrDefault((KeyValue p) => string.Equals(p.Key, propertyName, StringComparison.Ordinal)) ?? throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The task properties does not contain the property named '{0}'.", propertyName), "propertyName")).Value;
	}
}
