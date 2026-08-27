using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class EndpointNotFoundCommunicationMIPException : CommunicationMIPException
{
	public EndpointNotFoundCommunicationMIPException(string message)
		: base(message)
	{
	}

	public EndpointNotFoundCommunicationMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
