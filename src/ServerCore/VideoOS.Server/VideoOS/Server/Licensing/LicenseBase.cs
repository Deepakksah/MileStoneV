using System.Diagnostics;
using System.Xml;

namespace VideoOS.Server.Licensing;

public abstract class LicenseBase
{
	private string macAddress;

	public string MacAddress
	{
		[DebuggerStepThrough]
		get
		{
			return macAddress;
		}
		[DebuggerStepThrough]
		set
		{
			macAddress = value.ToUpperInvariant();
		}
	}

	public abstract string Type { get; }

	public abstract void WriteLicenseDetails(XmlTextWriter xmlTextWriter);

	protected LicenseBase(string macAddress)
	{
		MacAddress = macAddress.ToUpperInvariant();
	}
}
