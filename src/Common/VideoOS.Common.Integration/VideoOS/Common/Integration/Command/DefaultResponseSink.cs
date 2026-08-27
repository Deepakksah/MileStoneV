using System;

namespace VideoOS.Common.Integration.Command;

public class DefaultResponseSink : IResponseSink
{
	private class LocalResponseHandler : IResponseHandler
	{
		private DefaultResponseSink _responseSink;

		public LocalResponseHandler(DefaultResponseSink responseSink)
		{
			_responseSink = responseSink;
		}

		public void HandleResponse(CommandBase command, ResponseBase response)
		{
			_responseSink.AddResponse(response);
		}
	}

	private CommandBase _command;

	private ResponseDelegate _responseCallback;

	public DefaultResponseSink(CommandBase command, ResponseDelegate responseCallback)
	{
		_command = command;
		_responseCallback = responseCallback;
	}

	public void AddResponse(ResponseBase response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		response.Token = _command.Token;
		if (_responseCallback != null)
		{
			_responseCallback(_command, response);
		}
	}

	public void ProcessSync(CommandBase command, object state)
	{
		CommandProcessor.ProcessSync(command, state, new LocalResponseHandler(this));
	}

	public void ProcessAsync(CommandBase command, object state)
	{
		CommandProcessor.ProcessAsync(command, state, new LocalResponseHandler(this));
	}
}
