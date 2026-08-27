using System.Globalization;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class Discovery
{
	public const ushort NoPort = 0;

	[XmlElement("IpAddress")]
	public string Address { get; set; }

	[XmlElement("Validated")]
	public bool Validated { get; set; }

	[XmlElement("Port")]
	public ushort Port { get; set; }

	[XmlElement("UseSecurePort")]
	public bool UseSecurePort { get; set; }

	[XmlElement("SecurePort")]
	public ushort SecurePort { get; set; }

	[XmlElement("DeviceId")]
	public int DriverNumber { get; set; }

	[XmlElement("DllName")]
	public string DllName { get; set; }

	[XmlElement("DeviceTypeStr")]
	public string DeviceModelName { get; set; }

	public string DeviceModelNameWithEnding
	{
		get
		{
			string arg;
			switch (DriverNumber)
			{
			case 402:
			case 407:
				arg = "ONVIF";
				break;
			case 403:
			case 404:
				arg = "PSIA";
				break;
			default:
				return DeviceModelName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", DeviceModelName, arg);
		}
	}

	[XmlElement("UserName")]
	public string UserName { get; set; }

	[XmlElement("Password")]
	public string Password { get; set; }

	[XmlElement("MAC")]
	public string MacAddress { get; set; }

	[XmlElement("Product")]
	public string Product { get; set; }

	[XmlElement("MacExistsGlobal")]
	public bool MacExistsGlobal { get; set; }

	[XmlElement("MacExistsLocal")]
	public bool MacExistsLocal { get; set; }

	[XmlElement("Id")]
	public string ManufacturerId { get; set; }

	[XmlElement("CustomData")]
	public string CustomData { get; set; }

	[XmlElement("IsHomeSite")]
	public bool IsHomeSite { get; set; }

	[XmlElement("OutParams")]
	public string OutputParametersRaw
	{
		get
		{
			return OutputParameters?.ToJson() ?? string.Empty;
		}
		set
		{
			OutputParameters = DiscoveryOutputParameters.FromJson(value ?? string.Empty);
		}
	}

	[XmlIgnore]
	public DiscoveryOutputParameters OutputParameters { get; set; }
}
