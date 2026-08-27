using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class DiscoveryCollection : Collection<Discovery>
{
	public Discovery Add(string address, ushort port, bool useSecurePort, ushort securePort, int driverNumber, string driverName, string dllName)
	{
		Discovery discovery = new Discovery();
		discovery.Address = address;
		discovery.Port = port;
		discovery.UseSecurePort = useSecurePort;
		discovery.SecurePort = securePort;
		discovery.DriverNumber = driverNumber;
		discovery.DeviceModelName = driverName;
		discovery.DllName = dllName;
		discovery.Validated = false;
		Add(discovery);
		return discovery;
	}

	public Discovery Add(string address, ushort port, bool useSecurePort, ushort securePort, int driverNumber, string driverName, string dllName, string userName, string password, string macAddress, string product, bool validated, DiscoveryOutputParameters outputParameters)
	{
		Discovery discovery = new Discovery();
		discovery.Address = address;
		discovery.Port = port;
		discovery.UseSecurePort = useSecurePort;
		discovery.SecurePort = securePort;
		discovery.DriverNumber = driverNumber;
		discovery.DeviceModelName = driverName;
		discovery.DllName = dllName;
		discovery.UserName = userName;
		discovery.Password = password;
		discovery.MacAddress = macAddress;
		discovery.Product = product;
		discovery.Validated = validated;
		discovery.OutputParameters = outputParameters;
		Add(discovery);
		return discovery;
	}
}
