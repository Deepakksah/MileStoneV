using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Util;

public static class ServiceUtilCache
{
	private static Dictionary<Guid, List<Configuration.ServiceURIInfo>> _serviceCache = new Dictionary<Guid, List<Configuration.ServiceURIInfo>>();

	private static Dictionary<Guid, DateTime> _serviceFetchTime = new Dictionary<Guid, DateTime>();

	public static void StoreServiceUriInfoList(ServerId serverId, List<Configuration.ServiceURIInfo> serviceUriInfoList)
	{
		lock (_serviceCache)
		{
			_serviceCache[serverId.Id] = serviceUriInfoList;
			_serviceFetchTime[serverId.Id] = DateTime.UtcNow;
		}
	}

	public static bool GetServiceUriInfo(ServerId serverId, Guid serviceType, out Configuration.ServiceURIInfo? serviceUriInfo)
	{
		lock (_serviceCache)
		{
			serviceUriInfo = null;
			if (_serviceFetchTime.ContainsKey(serverId.Id) && _serviceFetchTime[serverId.Id] + TimeSpan.FromMinutes(10.0) < DateTime.UtcNow)
			{
				Flush(serverId);
				return false;
			}
			if (!_serviceCache.ContainsKey(serverId.Id))
			{
				return false;
			}
			foreach (Configuration.ServiceURIInfo item in _serviceCache[serverId.Id])
			{
				if (item.Type == serviceType)
				{
					serviceUriInfo = item;
					return true;
				}
			}
			return false;
		}
	}

	public static List<Configuration.ServiceURIInfo> GetServiceUriInfoList(ServerId serverId)
	{
		lock (_serviceCache)
		{
			if (_serviceFetchTime.ContainsKey(serverId.Id) && _serviceFetchTime[serverId.Id] + TimeSpan.FromMinutes(10.0) < DateTime.UtcNow)
			{
				Flush(serverId);
				return null;
			}
			if (!_serviceCache.ContainsKey(serverId.Id))
			{
				return null;
			}
			return _serviceCache[serverId.Id];
		}
	}

	public static void Flush()
	{
		lock (_serviceCache)
		{
			_serviceCache.Clear();
			_serviceFetchTime.Clear();
		}
	}

	public static void Flush(ServerId serverId)
	{
		lock (_serviceCache)
		{
			if (_serviceCache.ContainsKey(serverId.Id))
			{
				_serviceCache.Remove(serverId.Id);
			}
			if (_serviceFetchTime.ContainsKey(serverId.Id))
			{
				_serviceFetchTime.Remove(serverId.Id);
			}
		}
	}
}
