using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Communication;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business;

public static class DeviceStatusProvider
{
	private const int CacheRetention = 10;

	private static Dictionary<Guid, CacheEntry<List<DeviceStatus>>> DeviceStatusCacheByRecorderDic = new Dictionary<Guid, CacheEntry<List<DeviceStatus>>>();

	private static Dictionary<Guid, CacheEntry<List<DeviceStatus>>> DeviceStatusCacheByDevGroupDic = new Dictionary<Guid, CacheEntry<List<DeviceStatus>>>();

	private static Dictionary<Guid, CacheEntry<List<DeviceStatus>>> DeviceStatusCacheByDeviceDic = new Dictionary<Guid, CacheEntry<List<DeviceStatus>>>();

	public static DeviceStatusResult GetDeviceStatusByRecorderId(Guid recorderId, IServiceLogger serviceLogger)
	{
		return GetDeviceStatusInternal(recorderId, DeviceStatusCacheByRecorderDic, (DeviceOverview[] deviceOverviews) => Task.Factory.StartNew(delegate
		{
			RefreshDeviceStatus(recorderId, DeviceStatusCacheByRecorderDic, deviceOverviews, serviceLogger, "GetDeviceStatusByRecorderId");
		}, "QueryDeviceStatusByRecorderId task", TaskCreationOptions.LongRunning), () => Database.GetDeviceOverviewByRecorderId(recorderId), serviceLogger, "GetDeviceStatusByRecorderId");
	}

	public static DeviceStatusResult GetDeviceStatusByCameraGroupId(Guid cameraGroupId, IServiceLogger serviceLogger)
	{
		return GetDeviceStatusInternal(cameraGroupId, DeviceStatusCacheByDevGroupDic, (DeviceOverview[] deviceOverviews) => Task.Factory.StartNew(delegate
		{
			RefreshDeviceStatus(cameraGroupId, DeviceStatusCacheByDevGroupDic, deviceOverviews, serviceLogger, "GetDeviceStatusByCameraGroupId");
		}, "QueryDeviceStatusByCameraGroupId task", TaskCreationOptions.LongRunning), () => Database.GetDeviceOverviewByGroupId(cameraGroupId), serviceLogger, "GetDeviceStatusByCameraGroupId");
	}

	public static DeviceStatusResult GetDeviceStatusByCameraId(Guid cameraId, IServiceLogger serviceLogger)
	{
		return GetDeviceStatusInternal(cameraId, DeviceStatusCacheByDeviceDic, (DeviceOverview[] deviceOverviews) => Task.Factory.StartNew(delegate
		{
			RefreshDeviceStatus(cameraId, DeviceStatusCacheByDeviceDic, deviceOverviews, serviceLogger, "GetDeviceStatusByCameraId");
		}, "QueryDeviceStatusByCameraId task", TaskCreationOptions.LongRunning), () => Database.GetDeviceOverview(cameraId), serviceLogger, "GetDeviceStatusByCameraId");
	}

	private static DeviceStatusResult GetDeviceStatusInternal(Guid key, Dictionary<Guid, CacheEntry<List<DeviceStatus>>> deviceStatusCache, Func<DeviceOverview[], Task> createRefreshTaskFunc, Func<DeviceOverview[]> queryOverviewDataFunc, IServiceLogger serviceLogger, [CallerMemberName] string callerName = "")
	{
		Task task = null;
		DeviceStatusResult result;
		lock (deviceStatusCache)
		{
			if (deviceStatusCache.TryGetValue(key, out var value))
			{
				if (!value.PendingUpdate && DateTime.UtcNow - value.Timestamp > TimeSpan.FromSeconds(10.0))
				{
					value.PendingUpdate = true;
					DeviceOverview[] arg = queryOverviewDataFunc();
					task = createRefreshTaskFunc(arg);
				}
				result = CreateResult(value.Entry);
			}
			else
			{
				serviceLogger.LogToServerLog(FormattableString.Invariant($"{callerName} no cache yet for: '{key}'"), LogType.Information);
				DeviceOverview[] array = queryOverviewDataFunc();
				task = createRefreshTaskFunc(array);
				List<DeviceStatus> list = new List<DeviceStatus>(array.Select((DeviceOverview d) => ConvertDeviceOverviewToDeviceStatus(d)));
				value = new CacheEntry<List<DeviceStatus>>(list);
				value.PendingUpdate = true;
				deviceStatusCache[key] = value;
				result = new DeviceStatusResult
				{
					DeviceStatus = list.ToArray(),
					TotalUsedSpace = 0f,
					TotalUsedSpaceUnit = Unit.Unknown
				};
			}
		}
		if (task != null && task.Wait(TimeSpan.FromSeconds(2.0)))
		{
			lock (deviceStatusCache)
			{
				if (deviceStatusCache.TryGetValue(key, out var value2))
				{
					result = CreateResult(value2.Entry);
				}
			}
		}
		return result;
		static DeviceStatusResult CreateResult(IEnumerable<DeviceStatus> entries)
		{
			(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(entries.Sum((DeviceStatus e) => (long)e.UsedSpaceInBytesRaw), new Unit[4]
			{
				Unit.Bytes,
				Unit.KB,
				Unit.MB,
				Unit.GB
			}, 999L, 1024u);
			DeviceStatusResult obj = new DeviceStatusResult
			{
				DeviceStatus = entries.ToArray()
			};
			(obj.TotalUsedSpace, obj.TotalUsedSpaceUnit) = calculatedUnit;
			return obj;
		}
	}

	private static DeviceStatus ConvertDeviceOverviewToDeviceStatus(DeviceOverview deviceOverview)
	{
		return new DeviceStatus(deviceOverview.DeviceId)
		{
			DeviceName = deviceOverview.DeviceName,
			DeviceHostName = deviceOverview.DeviceHostName,
			RecordingServerName = deviceOverview.RecordingServerName,
			RecordingStorageName = deviceOverview.RecordingStorageName,
			ErrorState = ErrorState.StatusUnknown,
			RecorderId = deviceOverview.RecorderId,
			DataCollectorId = deviceOverview.DataCollectorId,
			DefaultLiveStreamFormat = VideoFormat.VideoFormatUnknown,
			RecordingStreamFormat = VideoFormat.VideoFormatUnknown,
			UsedSpace = 0f,
			UsedSpaceUnit = Unit.Unknown
		};
	}

	private static void RefreshDeviceStatus(Guid key, Dictionary<Guid, CacheEntry<List<DeviceStatus>>> deviceStatusCache, DeviceOverview[] deviceOverviews, IServiceLogger serviceLogger, string callerName)
	{
		try
		{
			serviceLogger.LogToServerLog(FormattableString.Invariant($"{callerName}, query status for: '{key}'"), LogType.Debug);
			List<DeviceStatus> deviceStatus = RecorderStatusService.GetDeviceStatus(ServerCommandServiceTokenAndUser.GetAppPoolSecurityToken(serviceLogger).Token, deviceOverviews, serviceLogger);
			UpdateDeviceStatusCache(key, deviceStatus, deviceStatusCache, serviceLogger);
		}
		catch (Exception ex)
		{
			serviceLogger.LogToServerLog(ex, FormattableString.Invariant($"{callerName}, an error occurred while quering device status"), LogType.Error);
			ResetUpdateCacheFlag(key, deviceStatusCache, serviceLogger);
		}
	}

	private static void UpdateDeviceStatusCache(Guid key, List<DeviceStatus> deviceStatusList, Dictionary<Guid, CacheEntry<List<DeviceStatus>>> deviceStatusCache, IServiceLogger serviceLogger)
	{
		lock (deviceStatusCache)
		{
			PurgeCache(deviceStatusCache, serviceLogger);
			deviceStatusCache[key] = new CacheEntry<List<DeviceStatus>>(deviceStatusList);
		}
	}

	private static void ResetUpdateCacheFlag(Guid key, Dictionary<Guid, CacheEntry<List<DeviceStatus>>> deviceStatusCache, IServiceLogger serviceLogger)
	{
		lock (deviceStatusCache)
		{
			CacheEntry<List<DeviceStatus>> value = null;
			if (deviceStatusCache.TryGetValue(key, out value))
			{
				value.PendingUpdate = false;
				value.UpdateTimestamp();
			}
			PurgeCache(deviceStatusCache, serviceLogger);
		}
	}

	private static void PurgeCache(Dictionary<Guid, CacheEntry<List<DeviceStatus>>> deviceStatusCache, IServiceLogger serviceLogger)
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid key in deviceStatusCache.Keys)
		{
			if (DateTime.UtcNow - deviceStatusCache[key].Timestamp > TimeSpan.FromSeconds(100.0))
			{
				list.Add(key);
			}
		}
		foreach (Guid item in list)
		{
			serviceLogger.LogToServerLog(FormattableString.Invariant($"DeviceStatus, purge status for: '{item}'"), LogType.Debug);
			deviceStatusCache.Remove(item);
		}
	}

	public static void ClearAllCache()
	{
		DeviceStatusCacheByRecorderDic.Clear();
		DeviceStatusCacheByDevGroupDic.Clear();
		DeviceStatusCacheByDeviceDic.Clear();
	}
}
