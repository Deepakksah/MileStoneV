using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common;

[Serializable]
public class ExceptionDialogException : Exception
{
	public ExceptionDialogException()
	{
	}

	public ExceptionDialogException(string message)
		: base(message)
	{
	}

	public ExceptionDialogException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ExceptionDialogException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
