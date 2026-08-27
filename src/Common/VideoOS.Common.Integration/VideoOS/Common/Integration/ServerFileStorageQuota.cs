using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Common.Integration;

public class ServerFileStorageQuota : IServerFileStorageQuota
{
	private readonly object _quotaLock = new object();

	private readonly Dictionary<Guid, ulong> _consumers = new Dictionary<Guid, ulong>();

	private ulong _totalUsage;

	public ulong Limit { get; }

	public ServerFileStorageQuota(ulong limit)
	{
		Limit = limit;
	}

	public void Free(Guid consumerId)
	{
		lock (_quotaLock)
		{
			if (_consumers.TryGetValue(consumerId, out var value))
			{
				_consumers.Remove(consumerId);
				_totalUsage -= value;
			}
		}
	}

	public bool Update(Guid consumerId, ulong consumerUsage)
	{
		lock (_quotaLock)
		{
			ulong totalUsageExcept = GetTotalUsageExcept(consumerId);
			if (totalUsageExcept + consumerUsage > Limit)
			{
				return false;
			}
			_totalUsage = totalUsageExcept + consumerUsage;
			_consumers[consumerId] = consumerUsage;
			return true;
		}
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "ServerFileStorageQuota with limit of {0} bytes.", Limit);
	}

	private ulong GetTotalUsageExcept(Guid excludedConsumerId)
	{
		if (_consumers.TryGetValue(excludedConsumerId, out var value))
		{
			return _totalUsage - value;
		}
		return _totalUsage;
	}
}
