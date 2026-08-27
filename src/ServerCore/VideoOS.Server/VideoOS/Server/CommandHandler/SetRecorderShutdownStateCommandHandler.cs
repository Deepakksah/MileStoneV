using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server.CommandHandler;

public sealed class SetRecorderShutdownStateCommandHandler : ICommandHandler
{
	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is SetRecorderShutdownStateCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			responseSink.AddResponse(new SuccessResponse());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to execute SetRecorderShutdownState command. Error: {ExceptionMessage}", ex2.Message);
			responseSink.AddResponse(new FailResponse());
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
