using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public abstract class GeminiServiceException : GeminiServerException
{
	public const int InvalidErrorCode = -1;

	private int _errorCode;

	private int _eventId;

	private EventLogEntryType _logLevel;

	private Dictionary<int, MessageParameter> _messageParameters = new Dictionary<int, MessageParameter>();

	public IGeminiRequestContext RequestContext { get; set; }

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

	public GeminiServiceException()
	{
	}

	public GeminiServiceException(string message)
		: base(message)
	{
	}

	public GeminiServiceException(int errorCode)
	{
		_errorCode = errorCode;
	}

	public GeminiServiceException(string message, int errorCode)
		: base(message)
	{
		_errorCode = errorCode;
	}

	public GeminiServiceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public GeminiServiceException(string message, int errorCode, Exception innerException)
		: base(message, innerException)
	{
		_errorCode = errorCode;
	}

	protected GeminiServiceException(SerializationInfo info, StreamingContext context)
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

	protected static string ExtractString(DatabaseError databaseError, string key)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		string text = "%" + key + "=";
		int num = databaseError.Message.IndexOf(text, StringComparison.CurrentCultureIgnoreCase);
		if (num == -1)
		{
			return string.Empty;
		}
		int num2 = databaseError.Message.IndexOf("%", num + 1, StringComparison.CurrentCultureIgnoreCase);
		if (num2 == -1)
		{
			return string.Empty;
		}
		return databaseError.Message.Substring(num + text.Length, num2 - (num + text.Length));
	}
}
