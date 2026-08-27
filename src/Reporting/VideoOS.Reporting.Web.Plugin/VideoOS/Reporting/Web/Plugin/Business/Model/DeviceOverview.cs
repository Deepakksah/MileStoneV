using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class DeviceOverview
{
	public Guid DeviceId { get; set; }

	public string DeviceName { get; set; }

	public string RecorderWebserverUri { get; set; }

	public string RecordingStorageName { get; set; }

	public string RecordingServerName { get; set; }

	public string DeviceHostName { get; set; }

	public Guid RecorderId { get; set; }

	public int DataCollectorId { get; set; }
}
