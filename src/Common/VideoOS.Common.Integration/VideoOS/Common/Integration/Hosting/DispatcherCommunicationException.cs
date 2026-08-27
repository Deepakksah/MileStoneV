using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class DispatcherCommunicationException : SecureTunnelServerException
{
	public string OriginalResponse { get; private set; }

	public DispatcherCommunicationException()
	{
	}

	public DispatcherCommunicationException(string message)
		: base(message)
	{
	}

	public DispatcherCommunicationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DispatcherCommunicationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		OriginalResponse = info.GetString("OriginalResponse");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("OriginalResponse", OriginalResponse);
		base.GetObjectData(info, context);
	}

	public DispatcherCommunicationException(string message, string originalResponse)
		: base(message)
	{
		OriginalResponse = originalResponse;
	}
}
