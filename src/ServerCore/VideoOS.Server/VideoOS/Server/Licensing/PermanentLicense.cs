using System;
using System.Xml;

namespace VideoOS.Server.Licensing;

public sealed class PermanentLicense : LicenseBase
{
	public string DeviceLicenseKey;

	public override string Type => "ffac8a74-85c4-4ce9-8f99-8ce7a790387d";

	public PermanentLicense(string macAddress, string deviceLicenseKey)
		: base(macAddress)
	{
		DeviceLicenseKey = deviceLicenseKey;
	}

	public override void WriteLicenseDetails(XmlTextWriter xmlTextWriter)
	{
		if (xmlTextWriter == null)
		{
			throw new ArgumentNullException("xmlTextWriter");
		}
		xmlTextWriter.WriteElementString("dlk", DeviceLicenseKey);
	}
}
