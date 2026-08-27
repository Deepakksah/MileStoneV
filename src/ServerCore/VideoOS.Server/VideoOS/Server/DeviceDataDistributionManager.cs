using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class DeviceDataDistributionManager : IDisposable
{
	public static class TestAccessor
	{
		public static void CleanupAll(DeviceDataDistributionManager instance)
		{
			instance.CleanupDeviceDataDistributionForAllRecorders();
		}
	}

	private const int SecondsBetweenCleanup = 60;

	private IServerEnvironment environment;

	private CancellationTokenSource cancellationTokenSource;

	private object sync = new object();

	private Timer cleanupTimer;

	private bool stop;

	public DeviceDataDistributionManager(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		environment = serverEnvironment;
		cleanupTimer = new Timer(CleanupTimerCallback);
		cancellationTokenSource = new CancellationTokenSource();
	}

	public void Start()
	{
		lock (sync)
		{
			environment.Server.Logger.LogDebug("Starting DeviceDataDistributionManager");
			cleanupTimer.Change(TimeSpan.FromSeconds(60.0), Timeout.InfiniteTimeSpan);
		}
	}

	public void Stop()
	{
		lock (sync)
		{
			stop = true;
			cleanupTimer.Change(-1, -1);
			cancellationTokenSource.Cancel();
		}
	}

	private void CleanupTimerCallback(object state)
	{
		try
		{
			environment.Server.Logger.LogDebug("DeviceDataDistributionManager cleanup timer callback called");
			CleanupDeviceDataDistributionForAllRecorders();
		}
		finally
		{
			lock (sync)
			{
				if (!stop)
				{
					cleanupTimer.Change(TimeSpan.FromSeconds(60.0), Timeout.InfiniteTimeSpan);
				}
			}
		}
	}

	private void CleanupDeviceDataDistributionForAllRecorders()
	{
		DeviceDataDistribution[] source;
		try
		{
			source = DeviceDataDistribution.QueryDeviceDataDistribution(environment.DataComponentFactory);
		}
		catch (GenericSqlException ex)
		{
			environment.Server.Logger.LogWarning("Device distribution manager failed to query device data distribution from the database. Exception: {Exception}", ex);
			return;
		}
		catch (DatabaseException ex2)
		{
			environment.Server.Logger.LogWarning("Device distribution manager failed to query device data distribution from the database. Exception: {Exception}", ex2);
			return;
		}
		foreach (IGrouping<Guid, DeviceDataDistribution> item in from d in source
			group d by d.RecorderId)
		{
			if (environment.Server.IsClosing)
			{
				break;
			}
			Guid key = item.Key;
			List<DeviceDataDistribution> distributionEntries = item.ToList();
			CleanupDeviceDataDistributionForRecorder(key, distributionEntries);
		}
	}

	private void CleanupDeviceDataDistributionForRecorder(Guid recorderId, IEnumerable<DeviceDataDistribution> distributionEntries)
	{
		IEnumerable<Guid> trackIds = distributionEntries.Select((DeviceDataDistribution e) => e.TrackId);
		IEnumerable<Guid> enumerable = QueryRecorderForTrackIdsToDelete(recorderId, trackIds);
		foreach (Guid trackIdToDelete in enumerable)
		{
			DeviceDataDistribution deviceDataDistribution = distributionEntries.Single((DeviceDataDistribution e) => e.TrackId == trackIdToDelete);
			try
			{
				environment.Server.Logger.LogInformation("Deleting device data distribution in SQL. Recorder id: {RecorderId}; Device id: {DeviceId}; Track: {TrackId}", recorderId, deviceDataDistribution.DeviceId, deviceDataDistribution.TrackId);
				DeviceDataDistribution.DeleteDeviceDataDistribution(environment.DataComponentFactory, recorderId, trackIdToDelete);
				Guid guid = Guid.Empty;
				try
				{
					guid = ConfigurationDataManager.QueryRecorderIdByDeviceId(environment.DataComponentFactory, deviceDataDistribution.DeviceId);
					if (guid != Guid.Empty)
					{
						environment.RecorderCommManager.TryProcessPendingTasksAsync(guid);
					}
					else
					{
						environment.Server.Logger.LogWarning("Failed to notify recorder about changed device data distribution. Could not find recorder from device id. Device id: {DeviceId}; Track: {TrackId}", deviceDataDistribution.DeviceId, deviceDataDistribution.TrackId);
					}
				}
				catch (DatabaseException ex)
				{
					environment.Server.Logger.LogWarning("Failed to notify recorder about changed device data distribution. Recorder id: {RecorderId}; Device id: {DeviceId}; Track: {TrackId}; Exception: {Exception}", guid, deviceDataDistribution.DeviceId, deviceDataDistribution.TrackId, ex);
				}
				environment.RecorderCommManager.TryProcessPendingTasksAsync(recorderId);
			}
			catch (VmoServerException ex2)
			{
				environment.Server.Logger.LogWarning("Failed to delete device data distribution entry. Recorder id: {RecorderId}; Device id: {DeviceId}; Track: {TrackId}; Exception: {Exception}", recorderId, deviceDataDistribution.DeviceId, deviceDataDistribution.TrackId, ex2);
			}
			catch (DatabaseException ex3)
			{
				environment.Server.Logger.LogWarning("Failed to delete device data distribution entry. Recorder id: {RecorderId}; Device id: {DeviceId}; Track: {TrackId}; Exception: {Exception}", recorderId, deviceDataDistribution.DeviceId, deviceDataDistribution.TrackId, ex3);
			}
		}
	}

	private IEnumerable<Guid> QueryRecorderForTrackIdsToDelete(Guid recorderId, IEnumerable<Guid> trackIds)
	{
		Guid guid = environment.RecorderCommManager.QueryRecorderServiceId(recorderId);
		if (guid == default(Guid))
		{
			environment.Server.Logger.LogDebug("Device data distribution manager. No recorder entry found for recorder. Recorder id: {RecorderId}", recorderId);
			return Enumerable.Empty<Guid>();
		}
		DatabaseOperationCommand databaseOperationCommand = new DatabaseOperationCommand();
		databaseOperationCommand.FilterTrackIdsToThoseWithoutMediaOnRecorder(recorderId, trackIds.ToArray());
		ICallContextInfo callContextInfo = environment.Server.CreateServerServiceCallContextUserInfo();
		IMessageManagerResponseCollector messageManagerResponseCollector = null;
		try
		{
			try
			{
				messageManagerResponseCollector = environment.MessageManager.SendCommand(guid, databaseOperationCommand, callContextInfo);
			}
			catch (RecorderNotFoundException)
			{
				environment.Server.Logger.LogDebug("Device data distribution manager. Recorder not found. Recorder id: {RecorderId}", recorderId);
				return Enumerable.Empty<Guid>();
			}
			catch (NotSupportedByLegacyRecorderException)
			{
				environment.Server.Logger.LogDebug("Device data distribution manager. Legacy Recorder does not support DatabaseOperationCommands. Recorder id: {RecorderId}", recorderId);
				return Enumerable.Empty<Guid>();
			}
			ResponseBase nextResponse;
			do
			{
				try
				{
					nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromMinutes(2.0));
				}
				catch (TimeoutException)
				{
					environment.Server.Logger.LogInformation("Device data distribution manager - timeout while waiting for response for database operation command. Recorder id: {RecorderId}", recorderId);
					return Enumerable.Empty<Guid>();
				}
				environment.Server.Logger.LogDebug("Device data distribution manager - Response received. Recorder id: {RecorderId}; Response: {Response}", recorderId, (nextResponse == null) ? "<NULL>" : nextResponse.GetType().Name);
			}
			while (nextResponse is HeartBeatResponse);
			if (nextResponse is DatabaseResponseTrackIdsWithoutMediaOnRecorder databaseResponseTrackIdsWithoutMediaOnRecorder)
			{
				return databaseResponseTrackIdsWithoutMediaOnRecorder.TrackIds;
			}
			return Enumerable.Empty<Guid>();
		}
		finally
		{
			messageManagerResponseCollector?.Dispose();
		}
	}

	public void Dispose()
	{
		bool flag = false;
		if (cleanupTimer != null)
		{
			using (ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false))
			{
				cleanupTimer.Dispose(manualResetEvent);
				flag = manualResetEvent.WaitOne(TimeSpan.FromMinutes(1.0));
			}
			cleanupTimer = null;
		}
		if (flag && cancellationTokenSource != null)
		{
			cancellationTokenSource.Dispose();
			cancellationTokenSource = null;
		}
	}
}
