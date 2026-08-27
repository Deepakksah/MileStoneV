using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.LPR.Common.WCF.DataContracts;

namespace VideoOS.LPR.Common.WCF.Client;

[Serializable]
public class ServiceFaultException : Exception
{
	public ErrorCode ErrorCode { get; private set; }

	public string Method { get; private set; }

	public ServiceFaultException(ErrorCode errorCode, string method, Exception innerException)
		: base(CreateExceptionMessage(errorCode, null, method, innerException), innerException)
	{
		ErrorCode = errorCode;
		Method = method;
	}

	public ServiceFaultException(ErrorCode errorCode, string errorMessage, string method, Exception innerException)
		: base(CreateExceptionMessage(errorCode, errorMessage, method, innerException), innerException)
	{
		ErrorCode = errorCode;
		Method = method;
	}

	private static string CreateExceptionMessage(ErrorCode errorCode, string errorMessage, string method, Exception innerException)
	{
		string text = ((innerException != null) ? string.Format(CultureInfo.InvariantCulture, "{0}: {1}", innerException.GetType().Name, innerException.Message) : string.Empty);
		if (!string.IsNullOrEmpty(errorMessage))
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} calling {1}: {2} {3}", errorCode, method, errorMessage, text);
		}
		return string.Format(CultureInfo.InvariantCulture, "{0} calling {1}. {2}", errorCode, method, text);
	}

	public ServiceFaultException()
	{
	}

	public ServiceFaultException(string message)
		: base(message)
	{
	}

	public ServiceFaultException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServiceFaultException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
