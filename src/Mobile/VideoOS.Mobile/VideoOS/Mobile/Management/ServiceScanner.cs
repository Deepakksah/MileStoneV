using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VideoOS.Mobile.Utilities;
using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.Management;

public class ServiceScanner
{
	private static DateTime _lastFlushDateTimeUtc = DateTime.MinValue;

	private static readonly TimeSpan _flushTimeout = TimeSpan.FromSeconds(5.0);

	public static List<VideoOS.Platform.Configuration.ServiceURIInfo> GetRegisteredServices(Guid serviceType)
	{
		FlushCache();
		return DynamicMethodInvoker.InvokeStaticMethod<List<VideoOS.Platform.Configuration.ServiceURIInfo>>(typeof(ServiceUtil), "GetRegisteredServiceUriInfo", new(Type, object)[2]
		{
			(typeof(Guid), serviceType),
			(typeof(ServerId), EnvironmentManager.Instance.CurrentSite.ServerId)
		}, "GetRegisteredServiceUriInfo", new(Type, object)[3]
		{
			(typeof(Guid), serviceType),
			(typeof(ServerId), EnvironmentManager.Instance.CurrentSite.ServerId),
			(typeof(NetworkCredential), EnvironmentManager.Instance.LoginNetworkCredential)
		});
	}

	private static void FlushCache()
	{
		if (DateTime.MinValue == _lastFlushDateTimeUtc)
		{
			_lastFlushDateTimeUtc = DateTime.UtcNow;
		}
		if (DateTime.UtcNow - _lastFlushDateTimeUtc >= _flushTimeout)
		{
			ServiceUtil.FlushCache();
			_lastFlushDateTimeUtc = DateTime.UtcNow;
		}
	}

	public static List<VideoOS.Platform.Configuration.ServiceURIInfo> RemoveItems(List<VideoOS.Platform.Configuration.ServiceURIInfo> services, List<Item> items)
	{
		List<VideoOS.Platform.Configuration.ServiceURIInfo> list = new List<VideoOS.Platform.Configuration.ServiceURIInfo>();
		foreach (VideoOS.Platform.Configuration.ServiceURIInfo service in services)
		{
			bool flag = true;
			foreach (Item item in items)
			{
				if (service.Instance == GetServerId(item))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				list.Add(service);
			}
		}
		return list;
	}

	public static VideoOS.Platform.Configuration.ServiceURIInfo GetServiceInfo(Guid serviceId, Guid serviceType)
	{
		return GetServiceInfo(serviceId, GetRegisteredServices(serviceType));
	}

	public static VideoOS.Platform.Configuration.ServiceURIInfo GetServiceInfo(Guid serviceId, List<VideoOS.Platform.Configuration.ServiceURIInfo> infos)
	{
		return infos.Find((VideoOS.Platform.Configuration.ServiceURIInfo x) => x.Instance == serviceId);
	}

	public static Guid GetServerId(Item item)
	{
		Guid result = Guid.Empty;
		try
		{
			result = new Guid(item.Properties["ServerId"]);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public static bool CheckItemForDelete(List<VideoOS.Platform.Configuration.ServiceURIInfo> services, Guid serverId)
	{
		return services.All((VideoOS.Platform.Configuration.ServiceURIInfo serviceUriInfo) => serviceUriInfo.Instance != serverId);
	}
}
