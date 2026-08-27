using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace VideoOS.Platform.Common.MessageClient;

public class CommandAcknowledgementHandler : ICommandAcknowledgementHandler
{
	private readonly ConcurrentDictionary<Guid, TaskCompletionSource<bool>> _commands = new ConcurrentDictionary<Guid, TaskCompletionSource<bool>>();

	private readonly TimeSpan _noAcknowledgementTimeout;

	public CommandAcknowledgementHandler(TimeSpan noAcknowledgementTimeout)
	{
		_noAcknowledgementTimeout = noAcknowledgementTimeout;
	}

	public async Task CommandResult(Guid commandId)
	{
		TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
		if (!_commands.TryAdd(commandId, taskCompletionSource))
		{
			throw new InvalidOperationMIPException($"commandId {commandId} already exists");
		}
		await taskCompletionSource.Task.TimeoutAfter("Awaiting command result", _noAcknowledgementTimeout).ConfigureAwait(continueOnCapturedContext: false);
		_commands.TryRemove(commandId, out var _);
	}

	public bool SetCommandResult(Guid commandId)
	{
		TaskCompletionSource<bool> value;
		bool result = _commands.TryRemove(commandId, out value);
		value?.SetResult(result: true);
		return result;
	}
}
