using System;
using System.Collections.Generic;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CommandHandler;

public sealed class VmoCache : IDisposable, IVmoCache
{
	private readonly VmoClient _client;

	private readonly Dictionary<Guid, TimeProfile> _cache;

	private bool _disposed;

	private readonly ServerConnection _connection;

	public VmoCache(IServerConfig config)
	{
		_connection = new ServerConnection(config.VmoCommunicationConfiguration.ServerName);
		_connection.ServerPort = config.VmoCommunicationConfiguration.Port;
		_client = new VmoClient(_connection);
		_cache = new Dictionary<Guid, TimeProfile>();
	}

	public string GetTimeProfileName(Guid profileId)
	{
		if (_cache.ContainsKey(profileId))
		{
			return _cache[profileId].Name;
		}
		TimeProfile timeProfile;
		try
		{
			timeProfile = _client.ManagementServer.TimeProfiles[profileId];
		}
		catch (VmoException)
		{
			return profileId.ToString();
		}
		_cache.Add(profileId, timeProfile);
		return timeProfile.Name;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				_client.Dispose();
				_connection.Dispose();
			}
			_disposed = true;
		}
	}
}
