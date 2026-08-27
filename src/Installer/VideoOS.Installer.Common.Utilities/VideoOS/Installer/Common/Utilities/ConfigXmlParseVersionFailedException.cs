using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Utilities;

[Serializable]
public class ConfigXmlParseVersionFailedException : Exception
{
	public ConfigXmlParseVersionFailedException()
	{
	}

	public ConfigXmlParseVersionFailedException(string message)
		: base(message)
	{
	}

	public ConfigXmlParseVersionFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ConfigXmlParseVersionFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
