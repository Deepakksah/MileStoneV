using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace VideoOS.Management.Common;

[Serializable]
public class VmoServerException : Exception
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

	public VmoServerException()
	{
	}

	public VmoServerException(string message)
		: base(message)
	{
	}

	public VmoServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected VmoServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
