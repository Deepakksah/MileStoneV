using System.ComponentModel;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class Credential
{
	public const string NoUser = "";

	[XmlElement("UserName")]
	[DefaultValue("")]
	public string UserName { get; set; }

	[XmlElement("Password")]
	public string Password { get; set; }

	public Credential()
	{
		UserName = string.Empty;
		Password = string.Empty;
	}

	public Credential(string password)
	{
		UserName = "";
		Password = password;
	}

	public Credential(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}
}
