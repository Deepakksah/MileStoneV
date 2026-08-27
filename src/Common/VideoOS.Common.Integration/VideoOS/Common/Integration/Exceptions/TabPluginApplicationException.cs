using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class TabPluginApplicationException : CorporateApplicationException
{
	public TabPluginApplicationException()
	{
	}

	public TabPluginApplicationException(string message)
		: base(message)
	{
	}

	public TabPluginApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected TabPluginApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
