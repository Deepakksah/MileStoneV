using System;

namespace VideoOS.Common.Integration.Command;

public interface ICommandSender
{
	void SendCommand(CommandBase command, object state, ResponseDelegate responseCallback);

	void DiscardResponse(Guid commandToken);
}
