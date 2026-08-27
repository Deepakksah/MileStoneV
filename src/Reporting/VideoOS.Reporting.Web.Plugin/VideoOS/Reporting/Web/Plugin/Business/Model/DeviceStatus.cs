using System;
using VideoOS.Reporting.Web.Plugin.Live;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DeviceStatus
{
	public Guid DeviceId { get; private set; }

	public string DeviceName { get; set; }

	public string RecordingStorageName { get; set; }

	public string RecordingServerName { get; set; }

	public ErrorState ErrorState { get; set; }

	public bool Recording { get; set; }

	public bool Motion { get; set; }

	public bool Started { get; set; }

	public bool Enabled { get; set; }

	public bool DbRepairInProgress { get; set; }

	public bool DbMoveInProgress { get; set; }

	public double KbpsValue { get; set; }

	public double DefaultLiveStreamFpsValue { get; set; }

	public double RecordingStreamFpsValue { get; set; }

	public VideoFormat DefaultLiveStreamFormat { get; set; }

	public VideoFormat RecordingStreamFormat { get; set; }

	public float UsedSpace { get; set; }

	public Unit UsedSpaceUnit { get; set; }

	public ulong UsedSpaceInBytesRaw { get; set; }

	public string DeviceHostName { get; set; }

	public Guid RecorderId { get; set; }

	public int DataCollectorId { get; set; }

	public DeviceStatus(Guid deviceId)
	{
		DeviceId = deviceId;
	}
}
