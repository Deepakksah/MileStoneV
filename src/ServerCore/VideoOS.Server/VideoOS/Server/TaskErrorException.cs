using System;
using System.Runtime.Serialization;

namespace VideoOS.Server;

[Serializable]
public class TaskErrorException : Exception
{
	public int ErrorCode { get; private set; }

	public bool IsFatal { get; private set; }

	public TaskErrorException()
	{
	}

	public TaskErrorException(string message)
		: base(message)
	{
	}

	public TaskErrorException(string message, Exception inner)
		: base(message, inner)
	{
	}

	public TaskErrorException(string message, int errorCode, bool isFatal)
		: base(message)
	{
		ErrorCode = errorCode;
		IsFatal = isFatal;
	}

	public TaskErrorException(string message, int errorCode, bool isFatal, Exception inner)
		: base(message, inner)
	{
		ErrorCode = errorCode;
		IsFatal = isFatal;
	}

	protected TaskErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		ErrorCode = info.GetInt32("TaskErrorException.ErrorCode");
		IsFatal = info.GetBoolean("TaskErrorException.IsFatal");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("TaskErrorException.ErrorCode", ErrorCode);
		info.AddValue("TaskErrorException.IsFatal", IsFatal);
		base.GetObjectData(info, context);
	}
}
