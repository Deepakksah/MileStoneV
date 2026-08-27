using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client;

public class PendingCommand : IDisposable
{
	private CommandHandler _commandHandler;

	private readonly ITimer _timer;

	private ulong _commandId;

	private readonly TaskCompletionSource<JObject> _responseTask;

	public PendingCommand(ulong commandId, TaskCompletionSource<JObject> responseTask, ITimer timer, TimeSpan timeoutTime, CommandHandler commandHandler)
	{
		_commandId = commandId;
		_responseTask = responseTask;
		_commandHandler = commandHandler;
		_timer = timer;
		timer.Elapsed += InactiveTimeout;
		timer.StartTimer(timeoutTime, Timeout.InfiniteTimeSpan);
	}

	private void InactiveTimeout(object sender, EventArgs e)
	{
		_commandHandler.CommandTimeout(_commandId);
	}

	public void ReturnResponse(JObject response, Exception exception)
	{
		if (exception != null)
		{
			_responseTask.TrySetException(exception);
		}
		else
		{
			_responseTask.TrySetResult(response);
		}
	}

	public void Dispose()
	{
		_timer.Dispose();
		_commandHandler = null;
	}
}
