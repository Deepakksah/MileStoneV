using System;

namespace VideoOS.Common.Integration;

public class ServiceConnectionStatus : IServiceConnectionStatus
{
	private readonly object _locker;

	private IServerConnectionProvider _serverConnection;

	private Guid _serviceId;

	private TimeSpan _timeout;

	private DateTime _lastConfirmedConnection;

	public TimeSpan Timeout
	{
		get
		{
			lock (_locker)
			{
				return _timeout;
			}
		}
		set
		{
			lock (_locker)
			{
				_timeout = value;
			}
		}
	}

	private DateTime LastConfirmedConnection
	{
		get
		{
			lock (_locker)
			{
				return _lastConfirmedConnection;
			}
		}
		set
		{
			lock (_locker)
			{
				_lastConfirmedConnection = value;
			}
		}
	}

	public event EventHandler<ServiceConnectionOnlineEventArgs> ServiceOnline;

	public event EventHandler<ServiceConnectionOfflineEventArgs> ServiceOffline;

	public event EventHandler<ServerCommunicationBrokenEventArgs> CommunicationBroken;

	public ServiceConnectionStatus(Guid serviceId, TimeSpan timeout, IServerConnectionProvider serverConnectionProvider)
	{
		if (timeout.TotalMilliseconds <= 0.0)
		{
			throw new ArgumentOutOfRangeException("timeout", "Timespan needs to be greater than 0");
		}
		_serviceId = serviceId;
		_timeout = timeout;
		_lastConfirmedConnection = DateTime.MinValue;
		_serverConnection = serverConnectionProvider;
		_locker = new object();
	}

	public void CheckTimeout()
	{
		if (_serverConnection.IsOnline && GetDateTimeUtcNow() - LastConfirmedConnection > Timeout)
		{
			LastConfirmedConnection = DateTime.MinValue;
			this.ServiceOffline?.Invoke(this, new ServiceConnectionOfflineEventArgs(_serviceId, timeout: true));
		}
	}

	public void CommunicationConfirmed()
	{
		LastConfirmedConnection = GetDateTimeUtcNow();
		if (!_serverConnection.IsOnline)
		{
			this.ServiceOnline?.Invoke(this, new ServiceConnectionOnlineEventArgs(_serviceId));
		}
	}

	public void CommunicationError()
	{
		LastConfirmedConnection = DateTime.MinValue;
		if (_serverConnection.IsOnline)
		{
			this.ServiceOffline?.Invoke(this, new ServiceConnectionOfflineEventArgs(_serviceId, timeout: false));
		}
	}

	public void AuthenticationError()
	{
		this.CommunicationBroken?.Invoke(this, new ServerCommunicationBrokenEventArgs(ServerCommunicationBrokenReason.ServiceAuthentication, _serviceId));
	}

	public void EndpointNotFound()
	{
		this.CommunicationBroken?.Invoke(this, new ServerCommunicationBrokenEventArgs(ServerCommunicationBrokenReason.EndpointNotFound, _serviceId));
	}

	public void InvalidCertificate()
	{
		this.CommunicationBroken?.Invoke(this, new ServerCommunicationBrokenEventArgs(ServerCommunicationBrokenReason.InvalidCertificate, _serviceId));
	}

	public static DateTime GetDateTimeUtcNow()
	{
		return DateTime.UtcNow;
	}
}
