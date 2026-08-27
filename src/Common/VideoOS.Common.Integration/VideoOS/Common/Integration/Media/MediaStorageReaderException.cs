using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Media;

[Serializable]
public class MediaStorageReaderException : Exception
{
	public bool WaitOnRetry { get; private set; }

	public MediaStorageReaderException()
	{
	}

	public MediaStorageReaderException(string message)
		: base(message)
	{
	}

	public MediaStorageReaderException(string message, bool waitOnRetry)
		: base(message)
	{
		WaitOnRetry = waitOnRetry;
	}

	public MediaStorageReaderException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public MediaStorageReaderException(string message, Exception inner, bool waitOnRetry)
		: base(message, inner)
	{
		WaitOnRetry = waitOnRetry;
	}

	protected MediaStorageReaderException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		WaitOnRetry = info.GetBoolean("WaitOnRetry");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("WaitOnRetry", WaitOnRetry);
		base.GetObjectData(info, context);
	}
}
