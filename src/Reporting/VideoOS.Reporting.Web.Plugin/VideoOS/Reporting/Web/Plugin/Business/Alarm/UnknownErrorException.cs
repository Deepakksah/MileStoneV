using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Alarm;

[Serializable]
public class UnknownErrorException : Exception
{
	public UnknownErrorException()
	{
	}

	public UnknownErrorException(string message)
		: base(message)
	{
	}

	public UnknownErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}
}
