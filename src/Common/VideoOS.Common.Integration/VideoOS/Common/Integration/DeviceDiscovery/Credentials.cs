using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.Crypto;

namespace VideoOS.Common.Integration.DeviceDiscovery;

[XmlRoot("Credentials")]
public class Credentials
{
	[XmlElement("Credential")]
	public CredentialCollection Items { get; set; }

	public Credentials()
	{
		Items = new CredentialCollection();
	}

	public Credentials(CredentialCollection collection)
	{
		Items = collection;
	}

	public string ToXml()
	{
		return XmlSerializationHelper.ToXml(this);
	}

	public string ToXmlEncrypted(ISymmetricEncryption symmetricEncryption)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ToXml());
		foreach (XmlNode item in xmlDocument.SelectNodes("Credentials/Credential/Password"))
		{
			string innerText = XmlSerializationHelper.Encrypt(item.InnerText, symmetricEncryption);
			item.InnerText = innerText;
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
		xmlTextWriter.Formatting = Formatting.Indented;
		xmlDocument.WriteTo(xmlTextWriter);
		return stringWriter.ToString();
	}

	public static Credentials FromXml(string xml)
	{
		return XmlSerializationHelper.FromXml<Credentials>(xml);
	}

	public static Credentials FromXmlEncrypted(string xml, ISymmetricEncryption symmetricEncryption)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		foreach (XmlNode item in xmlDocument.SelectNodes("Credentials/Credential/Password"))
		{
			string innerText = XmlSerializationHelper.Decrypt(item.InnerText, symmetricEncryption);
			item.InnerText = innerText;
		}
		return XmlSerializationHelper.FromXml<Credentials>(xmlDocument.OuterXml);
	}

	public Credentials Clone()
	{
		return FromXml(ToXml());
	}
}
