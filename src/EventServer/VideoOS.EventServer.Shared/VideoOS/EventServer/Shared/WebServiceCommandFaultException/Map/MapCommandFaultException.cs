using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class MapCommandFaultException : CommonCommandFaultException
{
	public MapCommandFaultException()
	{
	}

	public MapCommandFaultException(string errorDescription)
		: base(errorDescription)
	{
	}

	public MapCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, innerException)
	{
	}

	public MapCommandFaultException(string errorDescription, string errorReason)
		: base(errorDescription, errorReason)
	{
	}

	public MapCommandFaultException(string errorDescription, string errorReason, Exception innerException)
		: base(errorDescription, errorReason, innerException)
	{
	}
}
