using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressIntervalCollection : Collection<AddressInterval>
{
	public void Add(string dnsHostName, bool useSecurePort)
	{
		Add(new AddressInterval(dnsHostName, useSecurePort));
	}

	public void Add(string dnsHostName, ushort port, bool useSecurePort)
	{
		Add(new AddressInterval(dnsHostName, port, useSecurePort));
	}

	public void Add(string firstIPAddress, string lastIPAddress, bool useSecurePort)
	{
		Add(new AddressInterval(firstIPAddress, lastIPAddress, useSecurePort));
	}

	public void Add(string firstIPAddress, string lastIPAddress, ushort port, bool useSecurePort)
	{
		Add(new AddressInterval(firstIPAddress, lastIPAddress, port, useSecurePort));
	}

	public void Add(string dnsHostName, ushort firstPort, ushort lastPort, bool useSecurePort)
	{
		Add(new AddressInterval(dnsHostName, firstPort, lastPort, useSecurePort));
	}

	public void Add(string firstIPAddress, string lastIPAddress, ushort firstPort, ushort lastPort, bool useSecurePort)
	{
		Add(new AddressInterval(firstIPAddress, lastIPAddress, firstPort, lastPort, useSecurePort));
	}
}
