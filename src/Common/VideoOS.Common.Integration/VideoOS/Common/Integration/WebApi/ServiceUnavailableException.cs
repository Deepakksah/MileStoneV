using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.WebApi;

[Serializable]
public class ServiceUnavailableException : ResponseException
{
	public int RetryAfterInSeconds { get; set; } = -1;

	public ServiceUnavailableException()
	{
	}

	public ServiceUnavailableException(string message)
		: base(message)
	{
	}

	public ServiceUnavailableException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServiceUnavailableException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		RetryAfterInSeconds = (int)info.GetValue("RetryAfter", typeof(int));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("RetryAfter", RetryAfterInSeconds);
	}
}
