using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Hosting;

[Serializable]
public class UnknownRegisterException : SecureTunnelServerException
{
	public string OriginalResponse { get; private set; }

	public UnknownRegisterException()
	{
	}

	public UnknownRegisterException(string message)
		: base(message)
	{
	}

	public UnknownRegisterException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected UnknownRegisterException(SerializationInfo info, StreamingContext context)
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

	public UnknownRegisterException(string message, string originalResponse)
		: base(message)
	{
		OriginalResponse = originalResponse;
	}
}
