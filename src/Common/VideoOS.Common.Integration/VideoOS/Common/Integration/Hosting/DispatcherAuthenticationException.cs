using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class DispatcherAuthenticationException : SecureTunnelServerException
{
	public string OriginalResponse { get; private set; }

	public DispatcherAuthenticationException()
	{
	}

	public DispatcherAuthenticationException(string message)
		: base(message)
	{
	}

	public DispatcherAuthenticationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DispatcherAuthenticationException(SerializationInfo info, StreamingContext context)
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

	public DispatcherAuthenticationException(string message, string originalResponse)
		: base(message)
	{
		OriginalResponse = originalResponse;
	}
}
