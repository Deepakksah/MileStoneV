using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Exceptions;

[Serializable]
public class NavigationXmlApplicationException : CorporateApplicationException
{
	public NavigationXmlApplicationException()
	{
	}

	public NavigationXmlApplicationException(string message)
		: base(message)
	{
	}

	public NavigationXmlApplicationException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected NavigationXmlApplicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
