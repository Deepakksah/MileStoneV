using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressIntervalSelection
{
	[XmlElement("Selected")]
	public bool Selected { get; set; }

	[XmlElement("IpAddressInterval")]
	public AddressInterval AddressInterval { get; set; }

	public AddressIntervalSelection()
	{
	}

	public AddressIntervalSelection(bool selected, AddressInterval addressInterval)
	{
		Selected = selected;
		AddressInterval = addressInterval;
	}
}
