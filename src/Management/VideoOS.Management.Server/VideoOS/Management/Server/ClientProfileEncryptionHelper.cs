using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VideoOS.Management.Server;

public sealed class ClientProfileEncryptionHelper
{
	private ClientProfileEncryptionHelper()
	{
	}

	public static string ClientProfileSettingsDecrypt(string clientProfileSettingXml, IClientProfileSpecificationCacheManager specification, IGeminiRequestContext requestContext)
	{
		if (string.IsNullOrEmpty(clientProfileSettingXml))
		{
			return clientProfileSettingXml;
		}
		XDocument xDocument = XDocument.Parse(clientProfileSettingXml);
		IEnumerable<XElement> enumerable = from d in xDocument.Descendants("Setting")
			where specification.EncryptedClientProfileSettings.Any((string f) => f == d.Attribute("key").Value)
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
				string value2 = requestContext.GetEncryptionService().Decrypt(value);
				item.SetElementValue("Value", value2);
			}
		}
		return xDocument.ToString();
	}

	public static string ClientProfileSettingsEncrypt(string clientProfileSettingXml, IClientProfileSpecificationCacheManager specification, IGeminiRequestContext requestContext)
	{
		if (string.IsNullOrEmpty(clientProfileSettingXml))
		{
			return clientProfileSettingXml;
		}
		XDocument xDocument = XDocument.Parse(clientProfileSettingXml);
		IEnumerable<XElement> enumerable = from d in xDocument.Descendants("Setting")
			where specification.EncryptedClientProfileSettings.Any((string f) => f == d.Attribute("key").Value)
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
				item.SetElementValue("Value", requestContext.GetEncryptionService().Encrypt(value));
			}
		}
		return xDocument.ToString();
	}
}
