using System;
using System.Collections;
using System.Threading;

namespace VideoOS.Common.Integration.Command;

public class AsyncResponseSink : IResponseSink
{
	private static ManualResetEvent _shutdownEvent = new ManualResetEvent(initialState: false);

	private IResponseHandler _asyncResponseHandler;

	private bool _stopReceiving;

	private object _internalMembersLock = new object();

	private Hashtable _pendingCommands = new Hashtable();

	private ManualResetEvent _allResponsesReceivedEvent = new ManualResetEvent(initialState: true);

	public ICollection PendingCommandTokens
	{
		get
		{
			lock (_internalMembersLock)
			{
				return ((Hashtable)_pendingCommands.Clone()).Keys;
			}
		}
	}

	public AsyncResponseSink(IResponseHandler asyncResponseHandler)
	{
		_asyncResponseHandler = asyncResponseHandler;
	}

	public WaitResult WaitForResponses(TimeSpan timeout, bool breakOnShutdown)
	{
		WaitHandle[] waitHandles = ((!breakOnShutdown) ? new WaitHandle[1] { _allResponsesReceivedEvent } : new WaitHandle[2] { _allResponsesReceivedEvent, _shutdownEvent });
		int num = WaitHandle.WaitAny(waitHandles, timeout, exitContext: false);
		lock (_internalMembersLock)
		{
			_stopReceiving = true;
		}
		switch (num)
		{
		default:
			_ = 258;
			return WaitResult.WaitTimeout;
		case 0:
			return WaitResult.WaitOk;
		case 1:
			return WaitResult.Shutdown;
		}
	}

	public static void Shutdown()
	{
		_shutdownEvent.Set();
	}

	public void AddResponse(ResponseBase response)
	{
		if (response == null)
		{
			throw new ArgumentNullException("response");
		}
		CommandBase command = null;
		lock (_internalMembersLock)
		{
			if (_stopReceiving)
			{
				return;
			}
			command = _pendingCommands[response.Token] as CommandBase;
			_pendingCommands.Remove(response.Token);
		}
		if (_asyncResponseHandler != null)
		{
			_asyncResponseHandler.HandleResponse(command, response);
		}
		lock (_internalMembersLock)
		{
			if (_pendingCommands.Count == 0)
			{
				_allResponsesReceivedEvent.Set();
			}
		}
	}

	public void ProcessSync(CommandBase command, object state)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		lock (_internalMembersLock)
		{
			_allResponsesReceivedEvent.Reset();
			_pendingCommands[command.Token] = command;
		}
		CommandProcessor.ProcessSync(command, state, new ChainResponseSink(command, this));
	}

	public void ProcessAsync(CommandBase command, object state)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		lock (_internalMembersLock)
		{
			_allResponsesReceivedEvent.Reset();
			_pendingCommands[command.Token] = command;
		}
		CommandProcessor.ProcessAsync(command, state, new ChainResponseSink(command, this));
	}
}
