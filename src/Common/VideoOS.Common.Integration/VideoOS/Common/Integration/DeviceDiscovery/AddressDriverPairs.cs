using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("AddressDriverPairs")]
public class AddressDriverPairs
{
	[XmlElement("AddressDriverPair")]
	public AddressDriverPairCollection Items { get; set; }

	public AddressDriverPairs()
	{
		Items = new AddressDriverPairCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static AddressDriverPairs FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<AddressDriverPairs>(xml);
	}

	public AddressDriverPairs Clone()
	{
		return FromXml(ToXml());
	}
}
