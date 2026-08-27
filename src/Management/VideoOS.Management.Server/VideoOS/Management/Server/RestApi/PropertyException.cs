using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server.RestApi;

[Serializable]
[ExcludeFromCodeCoverage]
public class PropertyException : Exception
{
	public string PropertyKey { get; set; }

	[ExcludeFromCodeCoverage]
	public PropertyException()
	{
	}

	[ExcludeFromCodeCoverage]
	public PropertyException(string text)
		: base(text)
	{
		PropertyKey = "N/A";
	}

	[ExcludeFromCodeCoverage]
	public PropertyException(string propertyKey, string text)
		: base(text)
	{
		PropertyKey = propertyKey;
	}

	[ExcludeFromCodeCoverage]
	public PropertyException(string text, Exception exception)
		: base(text, exception)
	{
	}

	[ExcludeFromCodeCoverage]
	protected PropertyException(SerializationInfo serializationInfo, StreamingContext context)
		: base(serializationInfo, context)
	{
	}

	[ExcludeFromCodeCoverage]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue(GetType().Name + "PropertyKey", PropertyKey);
		base.GetObjectData(info, context);
	}
}
