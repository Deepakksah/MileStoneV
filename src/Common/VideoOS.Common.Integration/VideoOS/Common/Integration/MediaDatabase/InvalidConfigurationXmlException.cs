using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.MediaDatabase;

[Serializable]
public class InvalidConfigurationXmlException : Exception
{
	public InvalidConfigurationXmlException()
	{
	}

	public InvalidConfigurationXmlException(string message)
		: base(message)
	{
	}

	public InvalidConfigurationXmlException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidConfigurationXmlException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
