using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Communication;

namespace VideoOS.Reporting.Web.Plugin.Business;

public class ServerStatusProvider
{
	private const int CacheRetention = 10;

	private static Dictionary<Guid, CacheEntry<RecorderStatus>> RecorderStatusCacheDic = new Dictionary<Guid, CacheEntry<RecorderStatus>>();

	private static Dictionary<Guid, CacheEntry<FailoverStatus>> FailoverStatusCacheDic = new Dictionary<Guid, CacheEntry<FailoverStatus>>();

	public static RecorderStatus GetRecorderStatus(Guid recorderId, IServiceLogger serviceLogger)
	{
		Task task = null;
		RecorderStatus recorderStatus;
		lock (RecorderStatusCacheDic)
		{
			if (RecorderStatusCacheDic.TryGetValue(recorderId, out var value))
			{
				recorderStatus = value.Entry;
				if (!value.PendingUpdate && DateTime.UtcNow - value.Timestamp > TimeSpan.FromSeconds(10.0))
				{
					value.PendingUpdate = true;
					task = Task.Factory.StartNew(delegate
					{
						QueryRecorderStatus(recorderId, serviceLogger);
					}, "QueryRecorderStatus task", TaskCreationOptions.LongRunning);
				}
			}
			else
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "ServerStatus, no cache yet for recorder: '{0}'", recorderId), LogType.Information);
				recorderStatus = RecorderStatus.StatusUnknown;
				RecorderStatusCacheDic[recorderId] = new CacheEntry<RecorderStatus>(recorderStatus)
				{
					PendingUpdate = true
				};
				task = Task.Factory.StartNew(delegate
				{
					QueryRecorderStatus(recorderId, serviceLogger);
				}, "QueryRecorderStatus task", TaskCreationOptions.LongRunning);
			}
		}
		if (task != null && task.Wait(TimeSpan.FromSeconds(3.0)))
		{
			lock (RecorderStatusCacheDic)
			{
				if (RecorderStatusCacheDic.TryGetValue(recorderId, out var value2))
				{
					recorderStatus = value2.Entry;
				}
			}
		}
		return recorderStatus;
	}

	public static FailoverStatus GetFailoverStatus(Guid serverId, Uri uri, IServiceLogger serviceLogger)
	{
		Task task = null;
		FailoverStatus failoverStatus;
		lock (FailoverStatusCacheDic)
		{
			if (FailoverStatusCacheDic.TryGetValue(serverId, out var value))
			{
				failoverStatus = value.Entry;
				if (!value.PendingUpdate && DateTime.UtcNow - value.Timestamp > TimeSpan.FromSeconds(10.0))
				{
					value.PendingUpdate = true;
					task = Task.Factory.StartNew(delegate
					{
						QueryFailoverStatus(uri, serverId, serviceLogger);
					}, "QueryFailoverStatus task", TaskCreationOptions.LongRunning);
				}
			}
			else
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "ServerStatus, no cache yet for failover: '{0}'", uri), LogType.Information);
				failoverStatus = FailoverStatus.StatusUnknown;
				FailoverStatusCacheDic[serverId] = new CacheEntry<FailoverStatus>(failoverStatus)
				{
					PendingUpdate = true
				};
				task = Task.Factory.StartNew(delegate
				{
					QueryFailoverStatus(uri, serverId, serviceLogger);
				}, "QueryFailoverStatus task", TaskCreationOptions.LongRunning);
			}
		}
		if (task != null && task.Wait(TimeSpan.FromSeconds(3.0)))
		{
			lock (FailoverStatusCacheDic)
			{
				if (FailoverStatusCacheDic.TryGetValue(serverId, out var value2))
				{
					failoverStatus = value2.Entry;
				}
			}
		}
		return failoverStatus;
	}

	private static void QueryRecorderStatus(Guid recorderId, IServiceLogger serviceLogger)
	{
		try
		{
			serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "ServerStatus, query recorder status: '{0}'", recorderId), LogType.Debug);
			string recordingServerWebAddress = Database.GetRecordingServerWebAddress(recorderId);
			RecorderStatus recorderStatus = RecorderStatusService.GetRecorderStatus(ServerCommandServiceTokenAndUser.GetAppPoolSecurityToken(serviceLogger).Token, recordingServerWebAddress, serviceLogger);
			lock (RecorderStatusCacheDic)
			{
				PurgeCache(serviceLogger);
				RecorderStatusCacheDic[recorderId] = new CacheEntry<RecorderStatus>(recorderStatus);
			}
		}
		catch (Exception ex)
		{
			serviceLogger.LogToServerLog(ex, "Exception occurred while quering recorder status: " + ex.ToString(), LogType.Error);
			lock (RecorderStatusCacheDic)
			{
				CacheEntry<RecorderStatus> value = null;
				if (RecorderStatusCacheDic.TryGetValue(recorderId, out value))
				{
					value.PendingUpdate = false;
					value.UpdateTimestamp();
				}
				PurgeCache(serviceLogger);
			}
		}
	}

	private static void QueryFailoverStatus(Uri uri, Guid serverId, IServiceLogger serviceLogger)
	{
		try
		{
			FailoverStatus failoverStatus = null;
			try
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "ServerStatus, query failover status: '{0}'", uri.ToString()), LogType.Debug);
				failoverStatus = FailoverStatusService.GetFailoverStatus(uri);
			}
			catch (Exception ex)
			{
				failoverStatus = FailoverStatus.StatusServerUnavailable;
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "GetFailoverStatus for '{0}' failed: {1}", uri.ToString(), ex.Message), LogType.Error);
			}
			lock (FailoverStatusCacheDic)
			{
				List<Guid> list = new List<Guid>();
				foreach (Guid key in FailoverStatusCacheDic.Keys)
				{
					if (DateTime.UtcNow - FailoverStatusCacheDic[key].Timestamp > TimeSpan.FromSeconds(100.0))
					{
						list.Add(key);
					}
				}
				foreach (Guid item in list)
				{
					FailoverStatusCacheDic.Remove(item);
				}
				FailoverStatusCacheDic[serverId] = new CacheEntry<FailoverStatus>(failoverStatus);
			}
		}
		catch (Exception ex2)
		{
			serviceLogger.LogToServerLog(ex2, "Error occurred while quering failover status", LogType.Error);
			lock (FailoverStatusCacheDic)
			{
				CacheEntry<FailoverStatus> value = null;
				if (FailoverStatusCacheDic.TryGetValue(serverId, out value))
				{
					value.PendingUpdate = false;
				}
			}
		}
	}

	private static void PurgeCache(IServiceLogger serviceLogger)
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid key in RecorderStatusCacheDic.Keys)
		{
			if (DateTime.UtcNow - RecorderStatusCacheDic[key].Timestamp > TimeSpan.FromSeconds(100.0))
			{
				list.Add(key);
			}
		}
		foreach (Guid item in list)
		{
			serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "ServerStatus, purge cache for: '{0}'", item), LogType.Debug);
			RecorderStatusCacheDic.Remove(item);
		}
	}
}
