using System;

namespace VideoOS.Common.Integration.Command;

public class DirectCommandExecutor : ICommandExecutor
{
	public void Close(TimeSpan timeout)
	{
	}

	public void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink)
	{
		try
		{
			commandHandler.Execute(command, state, responseSink);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			CommandProcessor.LogUnhandledCommandHandlerException(command, ex2);
			responseSink.AddResponse(new ExceptionResponse(ex2));
		}
	}
}
