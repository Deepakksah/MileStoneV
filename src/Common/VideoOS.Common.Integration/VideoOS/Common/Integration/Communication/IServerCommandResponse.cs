using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

public interface IServerCommandResponse
{
	event EventHandler<ResponseReceivedEventArgs> ResponseReceived;

	event EventHandler<ResponseReceivedEventArgs> CommandFailed;

	void SendCommand(CommandBase command);

	bool WaitForServerResponse(Guid commandToken);

	bool WaitForServerResponse(Guid commandToken, TimeSpan timeout);
}
