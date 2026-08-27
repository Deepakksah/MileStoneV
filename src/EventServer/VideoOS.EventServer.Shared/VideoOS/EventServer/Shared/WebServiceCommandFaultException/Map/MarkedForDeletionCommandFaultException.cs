using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Map;

[Serializable]
public class MarkedForDeletionCommandFaultException : MapCommandFaultException
{
	public MarkedForDeletionCommandFaultException()
		: base("Marked For Deletion", MapCommandFaultExceptionErrorReason.MarkedForDeletion)
	{
	}

	public MarkedForDeletionCommandFaultException(string errorDescription)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.MarkedForDeletion)
	{
	}

	public MarkedForDeletionCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, MapCommandFaultExceptionErrorReason.MarkedForDeletion, innerException)
	{
	}
}
