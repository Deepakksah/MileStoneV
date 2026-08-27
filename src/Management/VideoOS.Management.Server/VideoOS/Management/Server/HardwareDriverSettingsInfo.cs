using System;

namespace VideoOS.Management.Server;

public sealed class HardwareDriverSettingsInfo
{
	public string HardwareId { get; set; }

	public string DefinitionDigest { get; set; }

	public string SettingsXml { get; set; }

	public DateTime LastModified { get; set; }
}
