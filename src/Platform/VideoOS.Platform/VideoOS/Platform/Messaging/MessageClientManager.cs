using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging.Internal;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

public sealed class MessageClientManager : IDisposable
{
	private class ClientWrapper
	{
		public Guid LoginSettingsInstanceId { get; set; }

		public IManagedMessageClient Client { get; set; }

		public int UnclosedStartCalls { get; set; }

		public Task StartTask { get; set; }
	}

	private static MessageClientManager _instance;

	private readonly Dictionary<Guid, ClientWrapper> _loginSettingsInstanceToClient = new Dictionary<Guid, ClientWrapper>();

	private readonly ReaderWriterLockSlim _clientsLock = new ReaderWriterLockSlim();

	private readonly EnvironmentManager _environmentManager;

	private readonly IInternalMessagesClientFactory _clientFactory;

	private readonly ILoginSettingsCacheWrapper _loginSettingsCache;

	private bool _disposed;

	public static MessageClientManager Instance
	{
		[DebuggerStepThrough]
		get
		{
			return GetInstance();
		}
	}

	public MessageClientManager(EnvironmentManager environmentManager, IInternalMessagesClientFactory clientFactory, ILoginSettingsCacheWrapper loginSettingsCache)
	{
		_environmentManager = environmentManager ?? throw new ArgumentNullMIPException("environmentManager");
		_clientFactory = clientFactory ?? throw new ArgumentNullMIPException("clientFactory");
		_loginSettingsCache = loginSettingsCache ?? throw new ArgumentNullMIPException("loginSettingsCache");
	}

	public async Task<IMessageClient> GetClientAsync(ServerId serverId, CancellationToken cancellationToken)
	{
		LoginSettings loginSettings = _loginSettingsCache.GetLoginSettings(serverId);
		await StartIfNotStartedAsync(loginSettings, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		_clientsLock.EnterReadLock();
		try
		{
			if (!_loginSettingsInstanceToClient.TryGetValue(loginSettings.InstanceGuid, out var value))
			{
				throw new MIPException("Client not found");
			}
			return value.Client;
		}
		finally
		{
			_clientsLock.ExitReadLock();
		}
	}

	public async Task StartAllUnStartedAsync(CancellationToken cancellationToken)
	{
		if (_environmentManager.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageClientManager", "Init");
		}
		List<Task> list = new List<Task>();
		_clientsLock.EnterReadLock();
		try
		{
			foreach (ClientWrapper value in _loginSettingsInstanceToClient.Values)
			{
				list.Add(TryStartAsync(value, cancellationToken));
			}
		}
		finally
		{
			_clientsLock.ExitReadLock();
		}
		await Task.WhenAll(list).ConfigureAwait(continueOnCapturedContext: false);
	}

	private async Task TryStartAsync(ClientWrapper clientWrapper, CancellationToken cancellationToken)
	{
		if (clientWrapper.StartTask == null)
		{
			clientWrapper.StartTask = clientWrapper.Client.StartAsync(cancellationToken);
			await clientWrapper.StartTask.ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public async Task CloseAllUnclosedAsync()
	{
		if (_environmentManager.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageClientManager", "Close");
		}
		while (true)
		{
			Guid guid = Guid.Empty;
			_clientsLock.EnterReadLock();
			try
			{
				if (_loginSettingsInstanceToClient.Count > 0)
				{
					guid = _loginSettingsInstanceToClient.Keys.ElementAt(0);
				}
				if (guid == Guid.Empty)
				{
					break;
				}
				if (_environmentManager.TraceMessageCommunication)
				{
					MipTrace.MgsCom("MessageClientManager", "CloseMessageCommunicationClient");
				}
			}
			finally
			{
				_clientsLock.ExitReadLock();
			}
			await CloseAsync(guid).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private async Task CloseAsync(Guid id)
	{
		_clientsLock.EnterReadLock();
		ClientWrapper clientWrapper;
		try
		{
			clientWrapper = _loginSettingsInstanceToClient[id];
		}
		finally
		{
			_clientsLock.ExitReadLock();
		}
		if (clientWrapper.StartTask != null)
		{
			await clientWrapper.Client.CloseAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		_clientsLock.EnterWriteLock();
		try
		{
			_loginSettingsInstanceToClient.Remove(id);
		}
		finally
		{
			_clientsLock.ExitWriteLock();
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			_disposed = true;
			_clientsLock.Dispose();
			GC.SuppressFinalize(this);
		}
	}

	private async Task StartIfNotStartedAsync(LoginSettings loginSettings, CancellationToken cancellationToken)
	{
		Task task = null;
		_clientsLock.EnterWriteLock();
		try
		{
			if (!_loginSettingsInstanceToClient.TryGetValue(loginSettings.InstanceGuid, out var value))
			{
				IManagedMessageClient managedMessageClient = _clientFactory.Create(loginSettings);
				value = new ClientWrapper
				{
					Client = managedMessageClient,
					LoginSettingsInstanceId = loginSettings.InstanceGuid,
					UnclosedStartCalls = 0,
					StartTask = (_environmentManager.ApplicationLoggedOn ? managedMessageClient.StartAsync(cancellationToken) : null)
				};
				_loginSettingsInstanceToClient.Add(loginSettings.InstanceGuid, value);
			}
			if (_environmentManager.TraceMessageCommunication)
			{
				MipTrace.MgsCom("MessageClientManager", "Start - " + loginSettings.InstanceGuid.ToString());
			}
			value.UnclosedStartCalls++;
			task = value.StartTask;
		}
		finally
		{
			_clientsLock.ExitWriteLock();
		}
		if (task != null)
		{
			await task.ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	private static MessageClientManager GetInstance()
	{
		if (EnvironmentManager.Instance == null)
		{
			throw new MIPException("Cannot access MessageClientManager before EnvironmentManager is instantiated.");
		}
		if (Configuration.Instance == null)
		{
			throw new MIPException("Cannot access MessageClientManager before Configuration is instantiated.");
		}
		if (InternalMessagesClientFactory.Instance == null)
		{
			throw new MIPException("Cannot access MessageClientManager before InternalMessagesClientFactory is instantiated.");
		}
		return _instance ?? (_instance = new MessageClientManager(EnvironmentManager.Instance, InternalMessagesClientFactory.Instance, new LoginSettingsCacheWrapper()));
	}
}
