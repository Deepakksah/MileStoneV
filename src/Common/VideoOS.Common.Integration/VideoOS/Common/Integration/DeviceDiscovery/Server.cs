using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class Server
{
	public const string NoUser = "";

	public const string NoPassword = "";

	[XmlElement("Type")]
	public string HostingType { get; set; }

	[XmlElement("Address")]
	public string Address { get; set; }

	[XmlElement("Port")]
	public int Port { get; set; }

	[XmlElement("UserName")]
	public string UserName { get; set; }

	[XmlElement("Password")]
	public string Password { get; set; }

	[XmlArray("HardwareCollection")]
	[XmlArrayItem("Hardware")]
	public HardwareInfoCollection Hardware { get; set; }

	public Server()
	{
		HostingType = string.Empty;
		Address = string.Empty;
		Port = 80;
		UserName = "";
		Password = "";
		Hardware = new HardwareInfoCollection();
	}

	public Server(string hostingType, string address, int port)
	{
		HostingType = hostingType;
		Address = address;
		Port = port;
		UserName = "";
		Password = "";
		Hardware = new HardwareInfoCollection();
	}

	public Server(string hostingType, string address, int port, string userName, string password)
	{
		HostingType = hostingType;
		Address = address;
		Port = port;
		UserName = userName;
		Password = password;
		Hardware = new HardwareInfoCollection();
	}
}
