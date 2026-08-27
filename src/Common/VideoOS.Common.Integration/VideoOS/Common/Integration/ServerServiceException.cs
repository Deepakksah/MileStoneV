using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class ServerServiceException : ServerException
{
	public enum FaultCode
	{
		Server,
		Client,
		VersionMismatch,
		MustUnderstand
	}

	private int _errorCode;

	public int ErrorCode
	{
		get
		{
			return _errorCode;
		}
		set
		{
			_errorCode = value;
		}
	}

	public ServerServiceException()
	{
	}

	public ServerServiceException(int errorCode)
	{
		_errorCode = errorCode;
	}

	public ServerServiceException(string message)
		: base(message)
	{
	}

	public ServerServiceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public ServerServiceException(string message, int errorCode)
		: base(message)
	{
		_errorCode = errorCode;
	}

	public ServerServiceException(string message, int errorCode, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerServiceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		_errorCode = info.GetInt32("_errorCode");
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("_errorCode", _errorCode);
		base.GetObjectData(info, context);
	}
}
