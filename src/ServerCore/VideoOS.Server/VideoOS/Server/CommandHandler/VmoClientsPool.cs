using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Management.VmoClient;
using VideoOS.Utilities.Timers;

namespace VideoOS.Server.CommandHandler;

public sealed class VmoClientsPool : IVmoClientFactory, IDisposable
{
	public sealed class VmoTimedClient : VmoClient
	{
		private DateTime _timeStamp = DateTime.MaxValue;

		public Guid Id { get; }

		public VmoTimedClient(string serverName, int serverPort)
			: base(new ServerConnection(serverName)
			{
				ServerPort = serverPort
			})
		{
			Id = Guid.NewGuid();
		}

		public void Initialize()
		{
			if (!base.ServerConnection.IsOpen)
			{
				base.ServerConnection.Connect();
			}
			_timeStamp = DateTime.UtcNow;
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (!(obj is VmoTimedClient vmoTimedClient))
			{
				return false;
			}
			return Id == vmoTimedClient.Id;
		}

		public bool DisposeIfOld(TimeSpan age)
		{
			bool flag = DateTime.UtcNow.Subtract(_timeStamp) >= age;
			if (flag)
			{
				Dispose();
			}
			return flag;
		}
	}

	private readonly TimeSpan _keepAliveTimeout;

	private readonly ILogger _logger;

	private readonly string _serverName;

	private readonly int _serverPort;

	private ITimer _timer;

	private readonly object _lock = new object();

	public readonly HashSet<VmoTimedClient> _poolBusy = new HashSet<VmoTimedClient>();

	public readonly HashSet<VmoTimedClient> _poolFree = new HashSet<VmoTimedClient>();

	private bool _disposedValue;

	public VmoClientsPool(string serverName, int serverPort, TimeSpan keepAliveTimeout, ILogger logger)
		: this(serverName, serverPort, keepAliveTimeout, logger, new ThreadingTimerWrapper())
	{
	}

	public VmoClientsPool(string serverName, int serverPort, TimeSpan keepAliveTimeout, ILogger logger, ITimer timer)
	{
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		if (timer == null)
		{
			throw new ArgumentNullException("timer");
		}
		_logger = logger;
		_serverName = serverName;
		_serverPort = serverPort;
		_keepAliveTimeout = keepAliveTimeout;
		_timer = timer;
		_timer.Elapsed += CleanUp;
	}

	public VmoTimedClient GetConnectedClient()
	{
		VmoTimedClient vmoTimedClient;
		lock (_lock)
		{
			if (_poolFree.Count > 0)
			{
				vmoTimedClient = _poolFree.First();
				_poolFree.Remove(vmoTimedClient);
			}
			else
			{
				vmoTimedClient = new VmoTimedClient(_serverName, _serverPort);
			}
			_poolBusy.Add(vmoTimedClient);
		}
		vmoTimedClient.Initialize();
		return vmoTimedClient;
	}

	public void ReleaseClient(VmoTimedClient client)
	{
		if (client == null)
		{
			throw new ArgumentNullException("client");
		}
		lock (_lock)
		{
			if (!_poolBusy.Remove(client))
			{
				_logger.LogError("Failed to release client {ClientId} - Unknown client", client.Id);
				return;
			}
			if (_poolFree.Count == 0)
			{
				_timer?.StartTimer(_keepAliveTimeout, _keepAliveTimeout);
			}
			_poolFree.Add(client);
		}
	}

	public void CleanUp(object source, EventArgs e)
	{
		lock (_lock)
		{
			int count = _poolFree.Count;
			if (count > 0)
			{
				_poolFree.RemoveWhere((VmoTimedClient c) => c.DisposeIfOld(_keepAliveTimeout));
				if (_poolFree.Count == 0)
				{
					_timer?.StopTimer();
				}
				int num = count - _poolFree.Count;
				if (num > 0)
				{
					_logger.LogDebug("Closed {ClosedCount} connections to Management Server. Active connections = {PoolBusyCount}. Passive connections ready for reuse = {PoolFree.Count}. KeepAlive = {TotalSeconds} seconds.", num, _poolBusy.Count, _poolFree.Count, _keepAliveTimeout.TotalSeconds);
				}
			}
		}
	}

	private void Dispose(bool disposing)
	{
		if (_disposedValue)
		{
			return;
		}
		if (disposing)
		{
			_timer.Dispose();
			lock (_lock)
			{
				foreach (VmoTimedClient item in _poolBusy)
				{
					item.Dispose();
				}
				foreach (VmoTimedClient item2 in _poolFree)
				{
					item2.Dispose();
				}
			}
		}
		_timer = null;
		_disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
