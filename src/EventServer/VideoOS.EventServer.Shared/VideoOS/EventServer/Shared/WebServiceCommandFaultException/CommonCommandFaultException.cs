using System;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException;

[Serializable]
public class CommonCommandFaultException : Exception
{
	private string _errorReason;

	public string ErrorDescription => base.Message;

	public string ErrorReason => _errorReason;

	public CommonCommandFaultException()
		: base(CommonCommandFaultExceptionErrorReason.Unknown)
	{
		_errorReason = CommonCommandFaultExceptionErrorReason.Unknown;
	}

	public CommonCommandFaultException(string errorDescription)
		: base(errorDescription)
	{
		_errorReason = CommonCommandFaultExceptionErrorReason.Unknown;
	}

	public CommonCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, innerException)
	{
		_errorReason = CommonCommandFaultExceptionErrorReason.Unknown;
	}

	public CommonCommandFaultException(string errorDescription, string errorReason)
		: base(errorDescription)
	{
		_errorReason = errorReason;
	}

	public CommonCommandFaultException(string errorDescription, string errorReason, Exception innerException)
		: base(errorDescription, innerException)
	{
		_errorReason = errorReason;
	}

	protected CommonCommandFaultException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		_errorReason = info.GetString("ErrorReason");
	}

	[Obsolete("This method should not be used in .NET 8 and upper")]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info == null)
		{
			throw new ArgumentException("Argument is null", "info");
		}
		base.GetObjectData(info, context);
		info.AddValue("ErrorReason", _errorReason);
	}
}
