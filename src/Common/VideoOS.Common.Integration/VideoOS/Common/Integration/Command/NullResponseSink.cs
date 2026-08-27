namespace VideoOS.Common.Integration.Command;

public class NullResponseSink : IResponseSink
{
	public void AddResponse(ResponseBase response)
	{
	}

	public void ProcessSync(CommandBase command, object state)
	{
		CommandProcessor.ProcessSync(command, state, CommandProcessor.NullResponseSink);
	}

	public void ProcessAsync(CommandBase command, object state)
	{
		CommandProcessor.ProcessAsync(command, state, CommandProcessor.NullResponseSink);
	}
}
