using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Platform.SDK.Multicast;

public static class MulticastController
{
	private static Dictionary<Guid, MulticastManager> _multicastManagers = new Dictionary<Guid, MulticastManager>();

	private static object _multicastManagersLock = new object();

	public static float MaxMulticastErrorRate = 0.25f;

	public static MulticastManager GetOrCreateMulticastManager(ServerId serverId)
	{
		MulticastManager value;
		lock (_multicastManagersLock)
		{
			if (!_multicastManagers.TryGetValue(serverId.Id, out value))
			{
				value = new MulticastManager(serverId);
				_multicastManagers.Add(serverId.Id, value);
				value.NoActiveRecordersEvent += MulticastManagerNoActiveRecordersEvent;
				value.Init();
				if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("MULTICASTERRORRATE", out var value2))
				{
					float.TryParse(value2, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out MaxMulticastErrorRate);
				}
			}
		}
		return value;
	}

	public static void MulticastManagerNoActiveRecordersEvent(MulticastManager multicastManager)
	{
		lock (_multicastManagersLock)
		{
			if (_multicastManagers.Remove(multicastManager.ServerId.Id))
			{
				multicastManager.Close();
			}
		}
	}
}
