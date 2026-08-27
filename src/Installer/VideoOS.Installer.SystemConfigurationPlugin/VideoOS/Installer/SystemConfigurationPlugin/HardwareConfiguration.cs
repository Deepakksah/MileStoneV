using System.Collections.Generic;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareConfiguration
{
	public string UserName { get; private set; }

	public string Password { get; private set; }

	public string DriverPath { get; private set; }

	public string IPAddress { get; private set; }

	public bool Validated { get; private set; }

	public string Name { get; set; }

	public string Id { get; set; }

	public IEnumerable<DeviceConfiguration> Devices { get; set; }

	public bool Added { get; set; }

	public HardwareConfiguration(string userName, string password, string driverPath, string ipAddress, bool validated, string name)
	{
		UserName = userName;
		Password = password;
		DriverPath = driverPath;
		IPAddress = ipAddress;
		Validated = validated;
		Name = name;
	}
}
