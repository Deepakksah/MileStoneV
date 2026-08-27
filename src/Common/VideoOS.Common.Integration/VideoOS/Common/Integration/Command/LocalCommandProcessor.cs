using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public class LocalCommandProcessor : ILocalCommandProcessor
{
	private class InternalResponseHandler<TCommand> : IResponseHandler where TCommand : CommandBase
	{
		private Action<TCommand, ResponseBase> responseCallback;

		public InternalResponseHandler(Action<TCommand, ResponseBase> responseCallback)
		{
			this.responseCallback = responseCallback;
		}

		public void HandleResponse(CommandBase command, ResponseBase response)
		{
			responseCallback(command as TCommand, response);
		}
	}

	public class ResponseCollector : IResponseCollector
	{
		private Dictionary<Guid, CommandBase> pendingCommands = new Dictionary<Guid, CommandBase>();

		private Queue<ResponseBase> responseQueue = new Queue<ResponseBase>();

		public ResponseCollector(CommandBase command)
		{
			pendingCommands.Add(command.Token, command);
		}

		public ResponseBase GetNextResponse(TimeSpan timeout)
		{
			using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			return GetNextResponse(timeout, cancellationTokenSource.Token);
		}

		public ResponseBase GetNextResponse(TimeSpan timeout, CancellationToken cancellationToken)
		{
			ResponseBase response = null;
			Task task = Task.Run(delegate
			{
				response = GetResponseFromQueue(timeout);
			});
			try
			{
				task.Wait(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				lock (responseQueue)
				{
					Monitor.Pulse(responseQueue);
				}
				throw new OperationCanceledException();
			}
			if (response == null)
			{
				throw new TimeoutException();
			}
			return response;
		}

		private ResponseBase GetResponseFromQueue(TimeSpan timeout)
		{
			lock (responseQueue)
			{
				if (responseQueue.Count > 0)
				{
					return responseQueue.Dequeue();
				}
				if (Monitor.Wait(responseQueue, timeout))
				{
					if (responseQueue.Count == 0)
					{
						return null;
					}
					return responseQueue.Dequeue();
				}
				return null;
			}
		}

		public void ProcessCommandCallback(CommandBase command, ResponseBase response)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			lock (this)
			{
				if (!pendingCommands.ContainsKey(response.Token))
				{
					return;
				}
				lock (responseQueue)
				{
					responseQueue.Enqueue(response);
					Monitor.Pulse(responseQueue);
				}
			}
		}
	}

	public static LocalCommandProcessor Instance { get; } = new LocalCommandProcessor();

	public IResponseCollector ProcessCommandAsync<TCommand>(TCommand command, object state) where TCommand : CommandBase
	{
		ResponseCollector responseCollector = new ResponseCollector(command);
		CommandProcessor.ProcessAsync(command, state, responseCollector.ProcessCommandCallback);
		return responseCollector;
	}

	public void ProcessCommandAsync<TCommand>(TCommand command, object state, Action<TCommand, ResponseBase> responseAction) where TCommand : CommandBase
	{
		if (responseAction == null)
		{
			throw new ArgumentNullException("responseAction");
		}
		InternalResponseHandler<TCommand> responseHandler = new InternalResponseHandler<TCommand>(responseAction);
		CommandProcessor.ProcessAsync(command, state, responseHandler);
	}
}
