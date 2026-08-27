using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Exceptions;

[Serializable]
public abstract class LocalizedException : Exception
{
	private Func<string> _lookupDisplayMessage;

	public string DisplayMessage => _lookupDisplayMessage?.Invoke() ?? Message;

	protected LocalizedException(string message, Func<string> lookupDisplayMessage, Exception innerException)
		: base(message, innerException)
	{
		_lookupDisplayMessage = lookupDisplayMessage;
	}

	protected LocalizedException(string message, Func<string> lookupDisplayMessage)
		: this(message, lookupDisplayMessage, null)
	{
	}

	protected LocalizedException()
	{
	}

	protected LocalizedException(string message)
		: base(message)
	{
	}

	protected LocalizedException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected LocalizedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
