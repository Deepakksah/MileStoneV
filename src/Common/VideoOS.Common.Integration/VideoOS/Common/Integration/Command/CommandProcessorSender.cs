using System;

namespace VideoOS.Common.Integration.Command;

public class CommandProcessorSender : ICommandSender
{
	public void SendCommand(CommandBase command, object state, ResponseDelegate responseCallback)
	{
		CommandProcessor.ProcessAsync(command, state, responseCallback);
	}

	public void DiscardResponse(Guid commandToken)
	{
	}
}
