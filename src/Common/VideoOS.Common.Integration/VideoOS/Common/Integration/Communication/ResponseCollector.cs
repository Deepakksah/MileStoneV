using System;
using System.Collections.Generic;
using System.Threading;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.Communication;

public class ResponseCollector : IDisposable
{
	private readonly Dictionary<Guid, CommandBase> pendingCommands = new Dictionary<Guid, CommandBase>();

	private readonly Queue<ResponseBase> responseQueue = new Queue<ResponseBase>();

	private readonly IServerCommandResponse serverCommandResponse;

	private bool disposed;

	public ResponseCollector(IServerCommandResponse serverCommandResponse)
	{
		if (serverCommandResponse == null)
		{
			throw new ArgumentNullException("serverCommandResponse");
		}
		this.serverCommandResponse = serverCommandResponse;
		ResponseReceivedWeakEventManager.AddHandler(this.serverCommandResponse, serverCommandResponse_ResponseReceived);
	}

	public void SendCommand(CommandBase command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		lock (pendingCommands)
		{
			serverCommandResponse.SendCommand(command);
			pendingCommands.Add(command.Token, command);
		}
	}

	public ResponseBase GetNextResponse(TimeSpan timeout)
	{
		lock (responseQueue)
		{
			if (responseQueue.Count > 0)
			{
				return responseQueue.Dequeue();
			}
			if (Monitor.Wait(responseQueue, timeout) && responseQueue.Count > 0)
			{
				return responseQueue.Dequeue();
			}
			return null;
		}
	}

	public void InterruptResponse()
	{
		Monitor.Pulse(responseQueue);
	}

	private void serverCommandResponse_ResponseReceived(object sender, ResponseReceivedEventArgs e)
	{
		lock (pendingCommands)
		{
			if (!pendingCommands.ContainsKey(e.Response.Token))
			{
				return;
			}
			lock (responseQueue)
			{
				responseQueue.Enqueue(e.Response);
				Monitor.Pulse(responseQueue);
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing)
			{
				ResponseReceivedWeakEventManager.RemoveHandler(serverCommandResponse, serverCommandResponse_ResponseReceived);
			}
			disposed = true;
		}
	}
}
