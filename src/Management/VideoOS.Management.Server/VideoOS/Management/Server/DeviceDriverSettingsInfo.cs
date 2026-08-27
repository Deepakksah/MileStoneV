using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class DeviceDriverSettingsInfo
{
	public string HardwareId { get; set; }

	public string DefinitionDigest { get; set; }

	public int DeviceIndex { get; set; }

	public DeviceType DeviceType { get; set; }

	public string SettingsXml { get; set; }

	public DateTime LastModified { get; set; }
}
