using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VideoOS.Common.Crypto;

namespace VideoOS.Management.Server;

public sealed class ClientProfileLegacyEncryptionHelper
{
	private ClientProfileLegacyEncryptionHelper()
	{
	}

	public static string LegacyClientProfileSettingsEncrypt(string clientProfileSettingXml)
	{
		if (string.IsNullOrEmpty(clientProfileSettingXml))
		{
			return clientProfileSettingXml;
		}
		XDocument xDocument = XDocument.Parse(clientProfileSettingXml);
		IEnumerable<XElement> enumerable = from d in xDocument.Descendants("Setting")
			where d.Attribute("key").Value.Equals("ExportSmartClientPlayerEncryptPassword", StringComparison.OrdinalIgnoreCase) || d.Attribute("key").Value.Equals("GisMapBingMapKey", StringComparison.OrdinalIgnoreCase) || d.Attribute("key").Value.Equals("GisMapGoogleMapClientId", StringComparison.OrdinalIgnoreCase) || d.Attribute("key").Value.Equals("GisMapGoogleMapPrivateKey", StringComparison.OrdinalIgnoreCase)
			select d;
		if (enumerable == null || enumerable.Count() == 0)
		{
			return clientProfileSettingXml;
		}
		foreach (XElement item in enumerable)
		{
			string value = item.Element("Value").Value;
			if (!string.IsNullOrEmpty(value))
			{
				string value2 = EncryptLegacy(value);
				item.SetElementValue("Value", value2);
			}
		}
		return xDocument.ToString();
	}

	private static EncryptionHelper CreateLegacyEncryptionHelper()
	{
		return new EncryptionHelper("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n                  <SymmetricAlgorithmConfiguration>\r\n                      <Algorithm>RC2</Algorithm>\r\n                      <Key>BUVZsX+bimNGkEWHC3Q84A==</Key>\r\n                      <IV>pKvqRM06QzQ=</IV>\r\n                      <Compression>Deflate</Compression>\r\n                  </SymmetricAlgorithmConfiguration>");
	}

	private static string EncryptLegacy(string setting)
	{
		return Convert.ToBase64String(CreateLegacyEncryptionHelper().EncryptStringData(setting));
	}
}
