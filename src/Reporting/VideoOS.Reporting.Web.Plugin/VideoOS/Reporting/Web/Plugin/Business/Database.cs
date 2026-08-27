using System;
using System.Collections.Generic;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.DataAccess;

namespace VideoOS.Reporting.Web.Plugin.Business;

public static class Database
{
	public static int GetMaxPhysicalMemory(int dataCollectorId)
	{
		using PerformanceCounterComponent performanceCounterComponent = new PerformanceCounterComponent();
		return performanceCounterComponent.GetMaxPhysicalMemory(dataCollectorId);
	}

	public static ServerOverview ServerOverview(Guid serverId)
	{
		using ServerOverviewComponent serverOverviewComponent = new ServerOverviewComponent();
		return serverOverviewComponent.GetServerOverview(serverId);
	}

	public static RecorderOverview RecorderOverview(Guid recorderId)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.GetRecorderOverview(recorderId);
	}

	public static DiskOverview[] DiskOverview(Guid recorderId)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.GetDiskOverview(recorderId);
	}

	public static AdapterOverview[] AdapterOverview(Guid recorderId)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.GetAdapterOverview(recorderId);
	}

	public static StorageOverview[] StorageOverview(Guid recorderId)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.GetStorageOverview(recorderId);
	}

	public static HistoryData[] GetHistoryData(Timescale timescale, int instanceId, int offsetMinutes)
	{
		using PerformanceCounterComponent performanceCounterComponent = new PerformanceCounterComponent();
		return performanceCounterComponent.GetHistoryData(timescale, instanceId, offsetMinutes);
	}

	public static DiskHistoryData[] GetDiskHistoryData(Timescale timescale, string driveName, Guid recorderId, int offsetMinutes, long maxValue)
	{
		using PerformanceCounterComponent performanceCounterComponent = new PerformanceCounterComponent();
		return performanceCounterComponent.GetDiskHistoryData(timescale, driveName, recorderId, offsetMinutes, maxValue);
	}

	public static RecorderName[] QueryRecorderNames(IEnumerable<Guid> recorderIds)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.QueryRecorderNames(recorderIds);
	}

	public static string GetRecordingServerWebAddress(Guid recorderId)
	{
		using RecorderOverviewComponent recorderOverviewComponent = new RecorderOverviewComponent();
		return recorderOverviewComponent.GetRecordingServerWebAddress(recorderId);
	}

	public static DeviceOverview[] GetDeviceOverviewByRecorderId(Guid recorderId)
	{
		using DeviceOverviewComponent deviceOverviewComponent = new DeviceOverviewComponent();
		return deviceOverviewComponent.GetDeviceOverviewByRecorder(recorderId);
	}

	public static DeviceOverview[] GetDeviceOverviewByGroupId(Guid groupId)
	{
		using DeviceOverviewComponent deviceOverviewComponent = new DeviceOverviewComponent();
		return deviceOverviewComponent.GetDeviceOverviewByGroup(groupId);
	}

	public static DeviceOverview[] GetDeviceOverview(Guid deviceId)
	{
		using DeviceOverviewComponent deviceOverviewComponent = new DeviceOverviewComponent();
		return deviceOverviewComponent.GetDeviceOverview(deviceId);
	}

	public static DeviceInstancesOverview GetDeviceInstancesOverview(Guid deviceId)
	{
		using DeviceOverviewComponent deviceOverviewComponent = new DeviceOverviewComponent();
		return deviceOverviewComponent.GetDeviceInstancesOverview(deviceId);
	}

	public static FailoverIncident[] GetFailoverIncidents(Guid failoverServerId, Timescale timescale)
	{
		using FailoverIncidenceComponent failoverIncidenceComponent = new FailoverIncidenceComponent();
		return failoverIncidenceComponent.GetFailoverIncidentsByFailoverServer(failoverServerId, timescale);
	}
}
