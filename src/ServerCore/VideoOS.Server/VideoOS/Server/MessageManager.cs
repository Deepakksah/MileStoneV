using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server;

public class MessageManager : IMessageManager, ICommandContainer
{
	private class CommandTimeoutEventArgs : EventArgs
	{
		private Guid commandToken;

		private System.Timers.Timer timer;

		public Guid CommandToken
		{
			[DebuggerStepThrough]
			get
			{
				return commandToken;
			}
		}

		public System.Timers.Timer SourceTimer
		{
			[DebuggerStepThrough]
			get
			{
				return timer;
			}
		}

		public CommandTimeoutEventArgs(Guid commandToken, System.Timers.Timer timer)
		{
			this.commandToken = commandToken;
			this.timer = timer;
		}
	}

	private class PendingCommand : IDisposable
	{
		private CommandBase command;

		private System.Timers.Timer timer;

		private IMessageManagerCallback messageManagerCallback;

		private TimeSpan responseTimeout;

		public CommandBase Command
		{
			[DebuggerStepThrough]
			get
			{
				return command;
			}
		}

		public System.Timers.Timer Timer
		{
			[DebuggerStepThrough]
			get
			{
				return timer;
			}
		}

		public event EventHandler<CommandTimeoutEventArgs> Timeout;

		private void OnTimeout(CommandTimeoutEventArgs e)
		{
			if (this.Timeout != null)
			{
				this.Timeout(this, e);
			}
		}

		public PendingCommand(CommandBase command, IMessageManagerCallback messageManagerCallback, TimeSpan responseTimeout)
		{
			this.command = command;
			this.messageManagerCallback = messageManagerCallback;
			this.responseTimeout = responseTimeout;
		}

		~PendingCommand()
		{
			Dispose(disposing: false);
		}

		public void HandleResponse(ResponseBase response)
		{
			messageManagerCallback.HandleResponse(command, response);
		}

		public bool IsLastExpectedResponse(ResponseBase response)
		{
			return messageManagerCallback.IsLastExpectedResponse(command, response);
		}

		public void HandleTimeout()
		{
			messageManagerCallback.HandleTimeout(command);
		}

		public void StartTimer()
		{
			CancelTimer();
			timer = new System.Timers.Timer();
			timer.AutoReset = false;
			timer.Interval = responseTimeout.TotalMilliseconds;
			timer.Elapsed += timer_Elapsed;
			timer.Start();
		}

		public void CancelTimer()
		{
			if (timer != null)
			{
				timer.Stop();
				timer.Elapsed -= timer_Elapsed;
				timer.Dispose();
				timer = null;
			}
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			OnTimeout(new CommandTimeoutEventArgs(command.Token, (System.Timers.Timer)sender));
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				CancelTimer();
			}
		}
	}

	public sealed class MessageManagerResponseCollector : IMessageManagerResponseCollector, IDisposable
	{
		private IRecorderCommManager recorderCommManager;

		private CommandBase command;

		private Queue<ResponseBase> queue;

		private bool disposedValue;

		public MessageManagerResponseCollector(CommandBase command, IRecorderCommManager recorderCommManager)
		{
			this.command = command;
			this.recorderCommManager = recorderCommManager;
			queue = new Queue<ResponseBase>();
			recorderCommManager.ResponseReceived += RecorderCommManager_ResponseReceived;
		}

		private void RecorderCommManager_ResponseReceived(object sender, ResponseEventArgs e)
		{
			if (e.Response.Token != command.Token)
			{
				return;
			}
			lock (queue)
			{
				queue.Enqueue(e.Response);
				Monitor.Pulse(queue);
			}
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
				response = GetNextResponseFromQueue(timeout);
			});
			try
			{
				task.Wait(cancellationToken);
			}
			catch (OperationCanceledException)
			{
				lock (queue)
				{
					Monitor.Pulse(queue);
				}
				throw new OperationCanceledException();
			}
			if (response == null)
			{
				throw new TimeoutException();
			}
			return response;
		}

		private ResponseBase GetNextResponseFromQueue(TimeSpan timeout)
		{
			lock (queue)
			{
				if (queue.Count > 0)
				{
					return queue.Dequeue();
				}
				if (Monitor.Wait(queue, timeout))
				{
					if (queue.Count == 0)
					{
						return null;
					}
					return queue.Dequeue();
				}
				return null;
			}
		}

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					recorderCommManager.ResponseReceived -= RecorderCommManager_ResponseReceived;
				}
				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	private IRecorderCommManager recorderCommManager;

	private Dictionary<Guid, PendingCommand> pendingCommands = new Dictionary<Guid, PendingCommand>();

	public MessageManager(IRecorderCommManager recorderCommManager)
	{
		this.recorderCommManager = recorderCommManager;
		this.recorderCommManager.ResponseReceived += RecorderCommManager_ResponseReceived;
	}

	private void RegisterCommand(CommandBase command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout)
	{
		PendingCommand pendingCommand = new PendingCommand(command, messageManagerCallback, timeout);
		pendingCommand.Timeout += PendingCommand_Timeout;
		lock (pendingCommands)
		{
			pendingCommands.Add(command.Token, pendingCommand);
			pendingCommand.StartTimer();
		}
	}

	private void UnregisterCommand(Guid token)
	{
		lock (pendingCommands)
		{
			if (pendingCommands.TryGetValue(token, out var value))
			{
				value.CancelTimer();
				pendingCommands.Remove(value.Command.Token);
				value.Timeout -= PendingCommand_Timeout;
				value.Dispose();
			}
		}
	}

	private void RecorderCommManager_ResponseReceived(object sender, ResponseEventArgs e)
	{
		PendingCommand value;
		lock (pendingCommands)
		{
			if (!pendingCommands.TryGetValue(e.Response.Token, out value))
			{
				return;
			}
			value.CancelTimer();
		}
		value.HandleResponse(e.Response);
		bool flag = value.IsLastExpectedResponse(e.Response);
		lock (pendingCommands)
		{
			if (flag)
			{
				pendingCommands.Remove(value.Command.Token);
				value.Timeout -= PendingCommand_Timeout;
				value.Dispose();
			}
			else
			{
				value.StartTimer();
			}
		}
	}

	private void PendingCommand_Timeout(object sender, CommandTimeoutEventArgs e)
	{
		bool flag = false;
		PendingCommand value;
		lock (pendingCommands)
		{
			if (!pendingCommands.TryGetValue(e.CommandToken, out value) || value.Timer != e.SourceTimer)
			{
				return;
			}
			flag = pendingCommands.Remove(e.CommandToken);
		}
		if (flag)
		{
			try
			{
				value.HandleTimeout();
			}
			finally
			{
				value.Timeout -= PendingCommand_Timeout;
				value.Dispose();
			}
		}
	}

	private IMessageManagerResponseCollector SendCommand(CommandBase command, Action sendAction)
	{
		MessageManagerResponseCollector messageManagerResponseCollector = new MessageManagerResponseCollector(command, recorderCommManager);
		try
		{
			sendAction();
			return messageManagerResponseCollector;
		}
		catch
		{
			messageManagerResponseCollector.Dispose();
			throw;
		}
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendUpdateSystemConfigurationCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendPrepareMoveHardwareCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, DatabaseOperationCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendDatabaseOperationCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendChangeHardwarePasswordCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendGetHardwareNetworkSettingsCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendSetHardwareNetworkSettingsCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, GetHardwareUsersCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendGetHardwareUsersCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, PingDeviceCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendPingDeviceCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, AddHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendAddHardwareUserCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, DeleteHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendDeleteHardwareUserCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendQueryDeviceEvidenceLockSizesCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendEvidenceLockOnOldRecorderCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendEvidenceLockOnOldRecorderCommand2(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendSetupInitialHardwareUserCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, FirmwareUpgradeCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendFirmwareUpgradeCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, FileUploadCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendFileUploadCommand(serviceId, command, callContextInfo);
		});
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendFilePresentOnRecorderCommand(serviceId, command, callContextInfo);
		});
	}

	public void SendCommand(Guid serviceId, HardwareSetupInformationCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendHardwareSetupInformationCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, HardwareScanCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendHardwareScanCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, GetAbsolutePtzPositionCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendGetAbsolutePtzPositionCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, PtzReserveSessionCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendPtzReserveSessionCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, PtzReleaseSessionCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendPtzReleaseSessionCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, PtzStopSessionCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendPtzStopSessionCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, SynchronizeDevicePresetsCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendSynchronizeDevicePresetsCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, DatabaseOperationCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendDatabaseOperationCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, ServerFileSystemOperationCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendServerFileSystemOperationCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, IMessageManagerCallback getNetworkSettingsCallback, TimeSpan timeout, ICallContextInfo contextInfo)
	{
		RegisterCommand(command, getNetworkSettingsCallback, timeout);
		try
		{
			recorderCommManager.SendGetHardwareNetworkSettingsCommand(serviceId, command, contextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, IMessageManagerCallback setNetworkSettingsCallback, TimeSpan timeout, ICallContextInfo contextInfo)
	{
		RegisterCommand(command, setNetworkSettingsCallback, timeout);
		try
		{
			recorderCommManager.SendSetHardwareNetworkSettingsCommand(serviceId, command, contextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public void SendCommand(Guid serviceId, GetHardwareUsersCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo)
	{
		RegisterCommand(command, messageManagerCallback, timeout);
		try
		{
			recorderCommManager.SendGetHardwareUsersCommand(serviceId, command, callContextInfo);
		}
		catch
		{
			UnregisterCommand(command.Token);
			throw;
		}
	}

	public IMessageManagerResponseCollector SendCommand(Guid serviceId, CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo)
	{
		return SendCommand(command, delegate
		{
			recorderCommManager.SendCheckExternalRecorderRegistrationCommand(serviceId, command, callContextInfo);
		});
	}

	CommandBase ICommandContainer.GetCommand(Guid token)
	{
		CommandBase result = null;
		lock (pendingCommands)
		{
			if (pendingCommands.TryGetValue(token, out var value))
			{
				result = value.Command;
			}
		}
		return result;
	}
}
