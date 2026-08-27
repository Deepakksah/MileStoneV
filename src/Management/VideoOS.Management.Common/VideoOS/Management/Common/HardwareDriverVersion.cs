using System;

namespace VideoOS.Management.Common;

public class HardwareDriverVersion
{
	public string DriverVersion { get; private set; }

	public DriverType DriverType { get; private set; }

	public string Revision { get; private set; }

	public HardwareDriverVersion(string driverVersion, string xmlVersion)
	{
		if (driverVersion == null)
		{
			throw new ArgumentNullException("driverVersion");
		}
		if (xmlVersion == null)
		{
			throw new ArgumentNullException("xmlVersion");
		}
		if (driverVersion.Contains("Device Pack") || driverVersion.Contains("DevicePack"))
		{
			DriverType = DriverType.DevicePack;
			if (driverVersion.Contains(", Flags:"))
			{
				int startIndex = driverVersion.IndexOf(", Flags:", StringComparison.OrdinalIgnoreCase);
				DriverVersion = driverVersion.Remove(startIndex);
			}
			else
			{
				DriverVersion = driverVersion.Replace("DevicePack", "Device Pack");
			}
		}
		if (driverVersion.Contains("Interconnected product driver"))
		{
			DriverType = DriverType.InterconnectedProduct;
			DriverVersion = driverVersion;
		}
		Revision = xmlVersion.Replace("$Revision: ", "").Replace(" $", "");
	}
}
