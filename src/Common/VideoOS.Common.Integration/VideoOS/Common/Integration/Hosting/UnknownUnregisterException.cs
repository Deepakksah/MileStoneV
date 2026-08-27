using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class UnknownUnregisterException : SecureTunnelServerException
{
	public string OriginalResponse { get; private set; }

	public UnknownUnregisterException()
	{
	}

	public UnknownUnregisterException(string message)
		: base(message)
	{
	}

	public UnknownUnregisterException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnknownUnregisterException(SerializationInfo info, StreamingContext context)
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

	public UnknownUnregisterException(string message, string originalResponse)
		: base(message)
	{
		OriginalResponse = originalResponse;
	}
}
