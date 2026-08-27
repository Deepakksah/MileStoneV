using System;
using System.Diagnostics;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.RecorderCommunication;

public sealed class ResponseEventArgs : EventArgs
{
	private ResponseBase response;

	public ResponseBase Response
	{
		[DebuggerStepThrough]
		get
		{
			return response;
		}
	}

	public ResponseEventArgs(ResponseBase response)
	{
		this.response = response;
	}
}
