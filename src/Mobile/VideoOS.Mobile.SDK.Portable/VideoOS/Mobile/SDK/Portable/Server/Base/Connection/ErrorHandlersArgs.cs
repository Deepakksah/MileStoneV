using System;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ErrorHandlersArgs : EventArgs
{
	public ErrorCodes ErrorCode { get; private set; }

	public CommunicationEntry Request { get; private set; }

	public CommunicationEntry Response { get; private set; }

	public ErrorHandlersArgs(ErrorCodes errorCode, CommunicationEntry request, CommunicationEntry response)
	{
		ErrorCode = errorCode;
		Request = request;
		Response = response;
	}
}
