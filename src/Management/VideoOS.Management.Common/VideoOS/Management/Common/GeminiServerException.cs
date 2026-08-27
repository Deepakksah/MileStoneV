using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace VideoOS.Management.Common;

[Serializable]
public class GeminiServerException : Exception
{
	public bool IsRemoteException => base.InnerException is FaultException<ExceptionDetail>;

	public string RemoteExceptionCode
	{
		get
		{
			if (base.InnerException is FaultException<ExceptionDetail> { Code: not null } ex && ex.Code.Name != null)
			{
				return ex.Code.Name;
			}
			return null;
		}
	}

	public GeminiServerException()
	{
	}

	public GeminiServerException(string message)
		: base(message)
	{
	}

	public GeminiServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected GeminiServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
