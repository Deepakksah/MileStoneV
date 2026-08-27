namespace VideoOS.Common.Integration.Command;

public interface ICommandProcessor
{
	void ProcessAsync(CommandBase command, object state);

	void ProcessAsync(CommandBase command, object state, IResponseSink responseSink);

	void ProcessSync(CommandBase command, object state);

	void ProcessSync(CommandBase command, object state, IResponseSink responseSink);
}
