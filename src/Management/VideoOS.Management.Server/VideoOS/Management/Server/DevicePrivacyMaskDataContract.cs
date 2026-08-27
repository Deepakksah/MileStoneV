using System;

namespace VideoOS.Management.Server;

public class DevicePrivacyMaskDataContract
{
	public string Id { get; set; }

	public DateTime LastModified { get; set; }

	public string PrivacyMaskXml { get; set; }

	public bool Enabled { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }
}
