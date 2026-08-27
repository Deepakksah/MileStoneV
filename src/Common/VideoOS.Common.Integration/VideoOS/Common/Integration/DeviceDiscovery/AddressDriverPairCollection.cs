using System;
using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressDriverPairCollection : Collection<AddressDriverPair>
{
	public AddressDriverPair Add(string address, ushort port)
	{
		return Add(address, port, -1, useSecurePort: false, 0);
	}

	public AddressDriverPair Add(string address, ushort port, int driverNumber, bool useSecurePort, ushort securePort)
	{
		if (driverNumber < -1)
		{
			throw new ArgumentOutOfRangeException("driverNumber", "Must be a value <= -1");
		}
		AddressDriverPair addressDriverPair = new AddressDriverPair();
		addressDriverPair.Address = address;
		addressDriverPair.Port = port;
		addressDriverPair.DriverNumber = driverNumber;
		addressDriverPair.UseSecurePort = useSecurePort;
		addressDriverPair.SecurePort = securePort;
		Add(addressDriverPair);
		return addressDriverPair;
	}
}
