using System;
using System.Collections.Generic;
using VideoOS.TrayController.Communication;
using VideoOS.TrayController.Communication.Server;
using VideoOS.Vms.Types;

namespace VideoOS.Server.Service;

public class ServerServiceState : IMessageProvider, IServerStateProvider
{
	private ServiceState _serviceState;

	private StatusMessageState _statusMessageState;

	private object SyncRoot = new object();

	public string ServiceStatusText
	{
		get
		{
			lock (SyncRoot)
			{
				return _serviceState.ToString();
			}
		}
	}

	public ServerServiceState()
	{
		_serviceState = ServiceState.Indeterminate;
		_statusMessageState = new StatusMessageState();
	}

	public void LogStatusMessage(DateTime timestamp, string message)
	{
		_statusMessageState.AddMessage(timestamp, message);
	}

	public void ChangeStatus(ServiceState status)
	{
		lock (SyncRoot)
		{
			_serviceState = status;
		}
	}

	public ServerState GetState()
	{
		return new ServerState
		{
			serviceState = _serviceState
		};
	}

	public IEnumerable<LogMessage> GetMessages(int sequenceNumber)
	{
		return _statusMessageState.GetMessages(sequenceNumber);
	}
}
