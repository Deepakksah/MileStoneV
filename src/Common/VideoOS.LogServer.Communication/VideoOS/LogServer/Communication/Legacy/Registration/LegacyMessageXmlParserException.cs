using System;

namespace VideoOS.LogServer.Communication.Legacy.Registration;

[Serializable]
public class LegacyMessageXmlParserException : Exception
{
	public LegacyMessageXmlParserException()
	{
	}

	public LegacyMessageXmlParserException(string message)
		: base(message)
	{
	}

	public LegacyMessageXmlParserException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
