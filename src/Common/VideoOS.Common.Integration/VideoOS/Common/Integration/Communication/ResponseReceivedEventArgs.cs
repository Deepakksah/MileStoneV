using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

public class ResponseReceivedEventArgs : EventArgs
{
	private ResponseBase _response;

	public ResponseBase Response => _response;

	public ResponseReceivedEventArgs(ResponseBase response)
	{
		_response = response;
	}
}
