using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using VideoOS.TrayController.Communication;
using VideoOS.TrayController.Communication.Server;
using VideoOS.Vms.Types;

namespace VideoOS.Server.Service;

public sealed class ServerObserver : IDisposable
{
	public sealed class ServerStateUpdatedEventArgs : EventArgs
	{
		public ServiceState State { get; }

		public ServerStateUpdatedEventArgs(ServerState state)
		{
			State = state.serviceState;
		}
	}

	public sealed class ServerMessagesReceivedEventArgs : EventArgs
	{
		public List<LogMessage> Messages;

		public bool InSession;

		public ServerMessagesReceivedEventArgs(IEnumerable<LogMessage> messages, bool inSession)
		{
			Messages = messages.ToList();
			InSession = inSession;
		}
	}

	private IServerTrayControllerClient _client;

	private Timer _timer;

	private object _timerLock = new object();

	private int _lastMessagesSequenceNumber;

	private bool _inSession;

	private int _portNumber;

	private bool _disposed;

	public TimeSpan PollPeriod { get; set; } = TimeSpan.FromSeconds(1.0);

	public event EventHandler<ServerStateUpdatedEventArgs> ServerStateUpdated;

	public event EventHandler ConnectionLost;

	public event EventHandler<ServerMessagesReceivedEventArgs> NewMessagesReceived;

	public ServerObserver(int portNumber)
	{
		_client = ServerTrayControllerFactory.GetClient(portNumber);
		_portNumber = portNumber;
		_inSession = false;
		_lastMessagesSequenceNumber = 0;
		_timer = new Timer(OnTimer);
	}

	public void Start()
	{
		_timer.Change(TimeSpan.Zero, Timeout.InfiniteTimeSpan);
	}

	private void OnTimer(object state)
	{
		try
		{
			if (!_inSession)
			{
				_lastMessagesSequenceNumber = 0;
			}
			List<LogMessage> messages = _client.GetMessages(_lastMessagesSequenceNumber);
			ServerMessagesReceivedEventArgs e = new ServerMessagesReceivedEventArgs(messages, _inSession);
			if (messages.Count != 0)
			{
				_lastMessagesSequenceNumber = messages.Last().SequenceNumber;
				this.NewMessagesReceived(this, e);
			}
			ServerState serverState = _client.GetServerState();
			ServerStateUpdatedEventArgs e2 = new ServerStateUpdatedEventArgs(serverState);
			this.ServerStateUpdated(this, e2);
			_inSession = true;
		}
		catch (IOException)
		{
			_client.CloseChannel();
			_client = null;
			_client = ServerTrayControllerFactory.GetClient(_portNumber);
			if (_inSession)
			{
				this.ConnectionLost(this, EventArgs.Empty);
				_inSession = false;
			}
		}
		finally
		{
			lock (_timerLock)
			{
				_timer?.Change(PollPeriod, Timeout.InfiniteTimeSpan);
			}
		}
	}

	public void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			Timer timer = _timer;
			lock (_timerLock)
			{
				_timer = null;
			}
			using (AutoResetEvent autoResetEvent = new AutoResetEvent(initialState: false))
			{
				timer.Dispose(autoResetEvent);
				autoResetEvent.WaitOne();
			}
			_client.CloseChannel();
			_client = null;
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
