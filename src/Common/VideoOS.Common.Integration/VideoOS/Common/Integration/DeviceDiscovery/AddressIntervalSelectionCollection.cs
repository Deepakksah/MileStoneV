using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressIntervalSelectionCollection : Collection<AddressIntervalSelection>
{
	public void Add(bool selected, string dnsHostName, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(dnsHostName, useSecurePort)));
	}

	public void Add(bool selected, string dnsHostName, ushort port, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(dnsHostName, port, useSecurePort)));
	}

	public void Add(bool selected, string firstIPAddress, string lastIPAddress, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(firstIPAddress, lastIPAddress, useSecurePort)));
	}

	public void Add(bool selected, string firstIPAddress, string lastIPAddress, ushort port, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(firstIPAddress, lastIPAddress, port, useSecurePort)));
	}

	public void Add(bool selected, string dnsHostName, ushort firstPort, ushort lastPort, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(dnsHostName, firstPort, lastPort, useSecurePort)));
	}

	public void Add(bool selected, string firstIPAddress, string lastIPAddress, ushort firstPort, ushort lastPort, bool useSecurePort)
	{
		Add(new AddressIntervalSelection(selected, new AddressInterval(firstIPAddress, lastIPAddress, firstPort, lastPort, useSecurePort)));
	}

	public void Add(bool selected, AddressInterval addressInterval)
	{
		Add(new AddressIntervalSelection(selected, addressInterval));
	}
}
