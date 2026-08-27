using System;

namespace VideoOS.Management.Server;

public class DeviceMotionDetectionDataContract
{
	public string Id { get; set; }

	public DateTime LastModified { get; set; }

	public bool MotionDetectionEnabled { get; set; }

	public string MotionDetectionXml { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }
}
