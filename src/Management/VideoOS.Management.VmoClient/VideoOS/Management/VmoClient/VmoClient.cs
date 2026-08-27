using System;

namespace VideoOS.Management.VmoClient;

public class VmoClient : IVmoClient, IDisposable
{
	private static readonly VmoClientEnvironment _environment = new VmoClientEnvironment();

	private static readonly VmoClient _vmoClientNotConnected = new VmoClient
	{
		_serverConnection = new ServerConnection
		{
			BlockUpdates = true
		}
	};

	private IServerConnection _serverConnection;

	private ManagementServer _managementServer;

	private VmoRepositories _repositories;

	private bool _disposed;

	public static VmoClientEnvironment Environment => _environment;

	public static VmoClient VmoClientNotConnected => _vmoClientNotConnected;

	public ManagementServer ManagementServer
	{
		get
		{
			if (_managementServer == null)
			{
				_managementServer = ((IVmoRepositories)Repositories).GetGenericRepository<ManagementServer>().GetObjectById("909749AE-88C4-4780-A381-383D691E98FA", refreshCachedObject: false, typeof(ManagementServer));
			}
			return _managementServer;
		}
		public set
		{
			_managementServer = value;
		}
	}

	public IServerConnection ServerConnection => _serverConnection;

	public VmoRepositories Repositories => _repositories;

	private VmoClient()
	{
		_repositories = new VmoRepositories(this);
		EntityNameAttributeHandlerWrapper.ScanAssemblyForClassAttributeEntityName();
	}

	public VmoClient(IServerConnection connection)
		: this()
	{
		_serverConnection = connection;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing && ServerConnection != null)
			{
				ServerConnection.Dispose();
			}
			_disposed = true;
		}
	}
}
