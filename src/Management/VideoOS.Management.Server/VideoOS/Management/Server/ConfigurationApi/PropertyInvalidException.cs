using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.ConfigurationApi;

[Serializable]
public class PropertyInvalidException : Exception
{
	public string PropertyName { get; private set; }

	public PropertyInvalidException()
		: base("The value of the property is invalid.")
	{
		PropertyName = string.Empty;
	}

	public PropertyInvalidException(string message)
		: base(message)
	{
		PropertyName = string.Empty;
	}

	public PropertyInvalidException(string message, Exception inner)
		: base(message, inner)
	{
		PropertyName = string.Empty;
	}

	public PropertyInvalidException(string message, string propertyName)
		: base(message)
	{
		PropertyName = propertyName;
	}

	public PropertyInvalidException(string message, string propertyName, Exception inner)
		: base(message, inner)
	{
		PropertyName = propertyName;
	}

	protected PropertyInvalidException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		PropertyName = info.GetString("PropertyInvalidException_PropertyName");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("PropertyInvalidException_PropertyName", PropertyName);
		base.GetObjectData(info, context);
	}
}
