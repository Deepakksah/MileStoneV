using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform.JsonHandling;

[Serializable]
[ExcludeFromCodeCoverage]
public class TokenException : Exception
{
	[ExcludeFromCodeCoverage]
	public TokenException()
	{
	}

	[ExcludeFromCodeCoverage]
	public TokenException(string text)
		: base(text)
	{
	}

	[ExcludeFromCodeCoverage]
	public TokenException(string text, Exception exception)
		: base(text, exception)
	{
	}

	[ExcludeFromCodeCoverage]
	protected TokenException(SerializationInfo serializationInfo, StreamingContext context)
	{
	}
}
