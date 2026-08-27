using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Alarm;

[Serializable]
public class InvalidTimeoutException : Exception
{
	public InvalidTimeoutException()
	{
	}

	public InvalidTimeoutException(string message)
		: base(message)
	{
	}

	public InvalidTimeoutException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
