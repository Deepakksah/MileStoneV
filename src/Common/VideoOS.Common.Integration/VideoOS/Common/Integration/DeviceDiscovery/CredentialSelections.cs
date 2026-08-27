using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.Integration.Login;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("CredentialSelections")]
public class CredentialSelections
{
	[XmlElement("UseDefaultCredential")]
	public bool UseDefaultCredential { get; set; }

	[XmlElement("CredentialSelection")]
	public CredentialSelectionCollection Items { get; set; }

	public CredentialSelections()
	{
		UseDefaultCredential = true;
		Items = new CredentialSelectionCollection();
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public static CredentialSelections FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<CredentialSelections>(xml);
	}

	public string ToXmlEncrypted()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ToXml());
		foreach (XmlNode item in xmlDocument.SelectNodes("CredentialSelections/CredentialSelection/Credential/Password"))
		{
			string innerText = DataProtection.Protect(item.InnerText);
			item.InnerText = innerText;
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlDocument.WriteTo(xmlTextWriter);
		return stringWriter.ToString();
	}

	public static CredentialSelections FromXmlEncrypted(string xml, IDataProtectionService dataProtectionService)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(xml);
		}
		catch (XmlException inner)
		{
			throw new LoadXmlException("Failed to parse file content as xml", inner);
		}
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("CredentialSelections/CredentialSelection/Credential/Password");
		if (xmlNodeList != null && xmlNodeList.Count > 0)
		{
			for (int num = xmlNodeList.Count - 1; num >= 0; num--)
			{
				XmlNode xmlNode = xmlNodeList[num];
				try
				{
					string innerText = xmlNode.InnerText;
					string innerText2 = dataProtectionService.Unprotect(innerText);
					xmlNode.InnerText = innerText2;
				}
				catch (Exception ex) when (ex is CryptographicException || ex is FormatException)
				{
					XmlNode xmlNode2 = xmlNode.SelectSingleNode("ancestor::CredentialSelection");
					xmlNode2?.ParentNode?.RemoveChild(xmlNode2);
				}
			}
		}
		return XmlSerializationHelper.FromXml<CredentialSelections>(xmlDocument.OuterXml);
	}

	public static CredentialSelections LoadXmlEncrypted(string fileName)
	{
		CredentialSelections result = new CredentialSelections();
		if (File.Exists(fileName))
		{
			try
			{
				result = FromXmlEncrypted(XmlSerializationHelper.LoadXmlString(fileName), new DataProtectionService());
			}
			catch (LoadXmlException)
			{
			}
		}
		return result;
	}

	public void SaveXmlEncrypted(string fileName)
	{
		string xml = ToXmlEncrypted();
		XmlSerializationHelper.SaveXmlString(fileName, xml);
	}

	public Credentials ExtractSelectedCredentials()
	{
		Credentials credentials = new Credentials();
		foreach (CredentialSelection item in Items.Where((CredentialSelection c) => c.Selected))
		{
			credentials.Items.Add(item.Credential);
		}
		return credentials;
	}

	public static void ConvertCredentials(string fileName, string fileNameLegacy)
	{
		LegacyFromXmlEncrypted(XmlSerializationHelper.LoadXmlString(fileNameLegacy)).SaveXmlEncrypted(fileName);
	}

	public static CredentialSelections LegacyFromXmlEncrypted(string xml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(xml);
		}
		catch (XmlException inner)
		{
			throw new LoadXmlException("Failed to parse file content as xml", inner);
		}
		foreach (XmlNode item in xmlDocument.SelectNodes("CredentialSelections/CredentialSelection/Credential/Password"))
		{
			string innerText = XmlSerializationHelper.LegacyDecrypt(item.InnerText);
			item.InnerText = innerText;
		}
		return XmlSerializationHelper.FromXml<CredentialSelections>(xmlDocument.OuterXml);
	}
}
