using System.Globalization;
using System.Xml.Serialization;

namespace VideoOS.Management.Common.Tasks;

public class HardwareScanResult
{
	[XmlElement("HardwareDriverId", DataType = "string")]
	public string InternalHardwareDriverId { get; set; }

	[XmlIgnore]
	public int HardwareDriverId
	{
		get
		{
			return int.Parse(InternalHardwareDriverId, CultureInfo.InvariantCulture);
		}
		set
		{
			InternalHardwareDriverId = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("UserName", DataType = "string")]
	public string InternalUserName { get; set; }

	[XmlIgnore]
	public string UserName
	{
		get
		{
			return InternalUserName;
		}
		set
		{
			InternalUserName = value;
		}
	}

	[XmlElement("Password", DataType = "string")]
	public string InternalPassword { get; set; }

	[XmlIgnore]
	public string Password
	{
		get
		{
			return InternalPassword;
		}
		set
		{
			InternalPassword = value;
		}
	}

	[XmlElement("PasswordEncrypted", DataType = "string")]
	public string InternalPasswordEncrypted { get; set; }

	[XmlIgnore]
	public bool PasswordEncrypted
	{
		get
		{
			return bool.Parse(InternalPasswordEncrypted);
		}
		set
		{
			InternalPasswordEncrypted = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("MacExistsGlobal", DataType = "string")]
	public string InternalMacExistsGlobal { get; set; }

	[XmlIgnore]
	public bool MacExistsGlobal
	{
		get
		{
			return bool.Parse(InternalMacExistsGlobal);
		}
		set
		{
			InternalMacExistsGlobal = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("MacExistsLocal", DataType = "string")]
	public string InternalMacExistsLocal { get; set; }

	[XmlIgnore]
	public bool MacExistsLocal
	{
		get
		{
			return bool.Parse(InternalMacExistsLocal);
		}
		set
		{
			InternalMacExistsLocal = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("MacAddress", DataType = "string")]
	public string InternalMacAddress { get; set; }

	[XmlIgnore]
	public string MacAddress
	{
		get
		{
			return InternalMacAddress;
		}
		set
		{
			InternalMacAddress = value;
		}
	}

	[XmlElement("IpAddress")]
	public string InternalAddress { get; set; }

	[XmlIgnore]
	public string Address
	{
		get
		{
			return InternalAddress;
		}
		set
		{
			InternalAddress = value;
		}
	}

	[XmlElement("Port", DataType = "string")]
	public string InternalPort { get; set; }

	[XmlIgnore]
	public int Port
	{
		get
		{
			return int.Parse(InternalPort, CultureInfo.InvariantCulture);
		}
		set
		{
			InternalPort = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("Validated", DataType = "string")]
	public string InternalValidated { get; set; }

	[XmlIgnore]
	public bool Validated
	{
		get
		{
			return bool.Parse(InternalValidated);
		}
		set
		{
			InternalValidated = value.ToString(CultureInfo.InvariantCulture);
		}
	}

	[XmlElement("InternalDiscoveryError", DataType = "string")]
	public string InternalDiscoveryError { get; set; }

	[XmlIgnore]
	public string DiscoveryError
	{
		get
		{
			return InternalDiscoveryError;
		}
		set
		{
			InternalDiscoveryError = value;
		}
	}
}
