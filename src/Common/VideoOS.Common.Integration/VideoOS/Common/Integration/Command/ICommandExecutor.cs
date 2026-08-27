using System;

namespace VideoOS.Common.Integration.Command;

public interface ICommandExecutor
{
	void Close(TimeSpan timeout);

	void Process(CommandBase command, object state, ICommandHandler commandHandler, IResponseSink responseSink);
}
