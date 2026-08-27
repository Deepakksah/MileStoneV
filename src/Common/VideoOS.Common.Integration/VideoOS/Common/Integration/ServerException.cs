using System;
using System.Runtime.Serialization;
using System.Web.Services.Protocols;

namespace VideoOS.Common.Integration;

[Serializable]
public class ServerException : Exception
{
	public bool IsRemoteException => base.InnerException is SoapException;

	public string RemoteExceptionCode
	{
		get
		{
			SoapException ex = base.InnerException as SoapException;
			if (ex.SubCode != null && ex.SubCode.Code != null && ex.SubCode.Code.Name != null)
			{
				return ex.SubCode.Code.Name;
			}
			return null;
		}
	}

	public ServerException()
	{
	}

	public ServerException(string message)
		: base(message)
	{
	}

	public ServerException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ServerException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
