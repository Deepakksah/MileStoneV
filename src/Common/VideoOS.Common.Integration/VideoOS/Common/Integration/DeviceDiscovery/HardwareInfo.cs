using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class HardwareInfo
{
	[XmlAttribute("Id")]
	public string ManufacturerId { get; set; }

	[XmlAttribute("UserName")]
	public string UserName { get; set; }

	[XmlAttribute("Password")]
	public string Password { get; set; }

	[XmlIgnore]
	public int Id { get; set; }

	[XmlIgnore]
	public string Name { get; set; }

	public HardwareInfo()
	{
		ManufacturerId = string.Empty;
		UserName = string.Empty;
		Password = string.Empty;
		Name = string.Empty;
	}

	public HardwareInfo(string manufacturerId)
	{
		ManufacturerId = manufacturerId;
		UserName = string.Empty;
		Password = string.Empty;
		Name = string.Empty;
	}

	public HardwareInfo(string manufacturerId, string userName, string password)
	{
		ManufacturerId = manufacturerId;
		UserName = userName;
		Password = password;
		Name = string.Empty;
	}
}
