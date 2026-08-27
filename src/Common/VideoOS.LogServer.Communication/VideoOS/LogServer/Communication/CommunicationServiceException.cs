using System;

namespace VideoOS.LogServer.Communication;

public class CommunicationServiceException : Exception
{
	public CommunicationExceptionMessage CommunicationExceptionMessage { get; }

	public CommunicationServiceException(CommunicationExceptionMessage communicationExceptionMessage)
	{
		CommunicationExceptionMessage = communicationExceptionMessage;
	}
}
