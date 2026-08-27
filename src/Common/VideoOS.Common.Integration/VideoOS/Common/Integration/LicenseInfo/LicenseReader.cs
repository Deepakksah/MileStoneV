using System;
using System.Xml;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public sealed class LicenseReader : ILicenseReader
{
	public string GetSkuFromLicense(string decryptedLicenseData)
	{
		if (string.IsNullOrEmpty(decryptedLicenseData))
		{
			throw new ArgumentNullException("decryptedLicenseData");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(decryptedLicenseData);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("//Software/Product[@SKU]");
		if (xmlNode != null && xmlNode.Attributes != null)
		{
			XmlAttribute xmlAttribute = xmlNode.Attributes["SKU"];
			if (xmlAttribute != null)
			{
				return xmlAttribute.Value;
			}
		}
		return string.Empty;
	}
}
