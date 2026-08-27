namespace VideoOS.Common.Integration.Command;

public class CommandProcessorProxy : ICommandProcessor
{
	private static CommandProcessorProxy _instance = new CommandProcessorProxy();

	public static CommandProcessorProxy Instance => _instance;

	public void ProcessAsync(CommandBase command, object state)
	{
		CommandProcessor.ProcessAsync(command, state, new NullResponseSink());
	}

	public void ProcessAsync(CommandBase command, object state, IResponseSink responseSink)
	{
		CommandProcessor.ProcessAsync(command, state, responseSink);
	}

	public void ProcessSync(CommandBase command, object state)
	{
		CommandProcessor.ProcessSync(command, state, new NullResponseSink());
	}

	public void ProcessSync(CommandBase command, object state, IResponseSink responseSink)
	{
		CommandProcessor.ProcessSync(command, state, responseSink);
	}
}
