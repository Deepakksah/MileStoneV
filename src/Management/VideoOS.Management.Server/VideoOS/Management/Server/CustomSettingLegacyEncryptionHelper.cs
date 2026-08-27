using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using VideoOS.Common.Crypto;

namespace VideoOS.Management.Server;

public sealed class CustomSettingLegacyEncryptionHelper
{
	private CustomSettingLegacyEncryptionHelper()
	{
	}

	public static string LegacyCustomSettingEncrypt(string customSettingXml)
	{
		if (string.IsNullOrEmpty(customSettingXml))
		{
			return customSettingXml;
		}
		XDocument xDocument = XDocument.Parse(customSettingXml);
		IEnumerable<XElement> enumerable = from d in xDocument.Descendants("Option")
			where d.Attribute("optionId").Value == "ExportSmartClientPlayerEncryptPassword" || d.Attribute("optionId").Value == "GisMapBingMapKey" || d.Attribute("optionId").Value == "GisMapGoogleMapClientId" || d.Attribute("optionId").Value == "GisMapGoogleMapPrivateKey"
			select d;
		if (enumerable == null || enumerable.Count() == 0)
		{
			return customSettingXml;
		}
		foreach (XElement item in enumerable)
		{
			string value = item.Attribute("value").Value;
			if (!string.IsNullOrEmpty(value))
			{
				string value2 = EncryptLegacy(value);
				item.SetAttributeValue("value", value2);
			}
		}
		return xDocument.ToString();
	}

	public static string LegacyCustomSettingDecrypt(string legacyEncryptedCustomSetting)
	{
		if (string.IsNullOrEmpty(legacyEncryptedCustomSetting))
		{
			return legacyEncryptedCustomSetting;
		}
		XDocument xDocument = XDocument.Parse(legacyEncryptedCustomSetting.ToString());
		foreach (XElement item in from d in xDocument.Descendants("Option")
			where d.Attribute("optionId").Value == "ExportSmartClientPlayerEncryptPassword" || d.Attribute("optionId").Value == "GisMapBingMapKey" || d.Attribute("optionId").Value == "GisMapGoogleMapClientId" || d.Attribute("optionId").Value == "GisMapGoogleMapPrivateKey"
			select d)
		{
			string value = item.Attribute("value").Value;
			if (!string.IsNullOrEmpty(value))
			{
				string value2 = DecryptLegacy(value);
				item.SetAttributeValue("value", value2);
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

	private static string DecryptLegacy(string encryptedSetting)
	{
		if (string.IsNullOrEmpty(encryptedSetting))
		{
			return encryptedSetting;
		}
		EncryptionHelper encryptionHelper = CreateLegacyEncryptionHelper();
		byte[] encrypted = Convert.FromBase64String(encryptedSetting);
		return encryptionHelper.DecryptStringData(encrypted);
	}
}
