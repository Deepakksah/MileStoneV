using System;

namespace VideoOS.Common.Integration.Command;

public class ChainResponseSink : IResponseSink
{
	private CommandBase _command;

	private IResponseSink _parentSink;

	public ChainResponseSink(CommandBase command, IResponseSink parentSink)
	{
		_command = command;
		_parentSink = parentSink;
	}

	public void AddResponse(ResponseBase response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		if (response.Token == Guid.Empty)
		{
			response.Token = _command.Token;
		}
		_parentSink.AddResponse(response);
	}

	public void ProcessSync(CommandBase command, object state)
	{
		_parentSink.ProcessSync(command, state);
	}

	public void ProcessAsync(CommandBase command, object state)
	{
		_parentSink.ProcessAsync(command, state);
	}
}
