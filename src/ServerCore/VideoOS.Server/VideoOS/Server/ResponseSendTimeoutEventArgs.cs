using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public sealed class ResponseSendTimeoutEventArgs : EventArgs
{
	private ResponseBase[] responses;

	public ResponseBase[] Responses => responses;

	public ResponseSendTimeoutEventArgs(ResponseBase[] responses)
	{
		this.responses = responses;
	}
}
