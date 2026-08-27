using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
public abstract class VmoServiceException : VmoServerException
{
	public const int InvalidErrorCode = -1;

	private int _errorCode;

	private int _eventId;

	private EventLogEntryType _logLevel;

	private Dictionary<int, MessageParameter> _messageParameters = new Dictionary<int, MessageParameter>();

	public int ErrorCode
	{
		get
		{
			if (_errorCode == 0)
			{
				return ErrorCodeDefault;
			}
			return _errorCode;
		}
		set
		{
			_errorCode = value;
		}
	}

	protected abstract int ErrorCodeDefault { get; }

	public int EventId
	{
		get
		{
			return _eventId;
		}
		set
		{
			_eventId = value;
		}
	}

	public EventLogEntryType LogLevel
	{
		get
		{
			return _logLevel;
		}
		set
		{
			_logLevel = value;
		}
	}

	public Dictionary<int, MessageParameter> MessageParameters
	{
		get
		{
			return _messageParameters;
		}
		private set
		{
			_messageParameters = value;
		}
	}

	public VmoServiceException()
	{
	}

	public VmoServiceException(string message)
		: base(message)
	{
	}

	public VmoServiceException(int errorCode)
	{
		_errorCode = errorCode;
	}

	public VmoServiceException(string message, int errorCode)
		: base(message)
	{
		_errorCode = errorCode;
	}

	public VmoServiceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public VmoServiceException(string message, int errorCode, Exception innerException)
		: base(message, innerException)
	{
		_errorCode = errorCode;
	}

	protected VmoServiceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}

	public string GetVMOErrorId()
	{
		return "VMO" + ErrorCode.ToString(CultureInfo.InvariantCulture);
	}
}
