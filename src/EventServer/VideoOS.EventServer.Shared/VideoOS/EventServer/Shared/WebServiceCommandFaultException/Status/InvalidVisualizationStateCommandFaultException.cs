using System;

namespace VideoOS.EventServer.Shared.WebServiceCommandFaultException.Status;

[Serializable]
public class InvalidVisualizationStateCommandFaultException : StatusCommandFaultException
{
	public InvalidVisualizationStateCommandFaultException()
		: base("Invalid Visualization State", StatusCommandFaultExceptionErrorReason.InvalidVisualizationState)
	{
	}

	public InvalidVisualizationStateCommandFaultException(string errorDescription)
		: base(errorDescription, StatusCommandFaultExceptionErrorReason.InvalidVisualizationState)
	{
	}

	public InvalidVisualizationStateCommandFaultException(string errorDescription, Exception innerException)
		: base(errorDescription, StatusCommandFaultExceptionErrorReason.InvalidVisualizationState, innerException)
	{
	}
}
