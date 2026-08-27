using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Mobile.Configuration;

[XmlRoot(ElementName = "InstConf")]
public class InstallerConfiguration
{
	[XmlElement("a")]
	public string LoggingFilePath { get; set; }

	[XmlElement("b")]
	public List<string> OsmAddresses { get; set; } = new List<string>();

	[XmlElement("c")]
	public string ServerId { get; set; }

	[XmlElement("d")]
	public string ServerIdS { get; set; }

	[XmlElement("e")]
	public string ServerIdT { get; set; }

	[XmlElement("f")]
	public string AccessControlAllowOrigins { get; set; }

	[XmlElement("g")]
	public string ExportsFolder { get; set; }

	[XmlElement("h")]
	public string CertificationPassword { get; set; }

	[XmlElement("n")]
	public string Uri { get; set; } = "http://localhost:80";

	[XmlElement("o")]
	public string AbsServiceHost { get; set; }

	[XmlElement("p")]
	public string OnlineNotificationServiceHost { get; set; }

	public static InstallerConfiguration FromXml(string xml)
	{
		if (string.IsNullOrWhiteSpace(xml))
		{
			return new InstallerConfiguration();
		}
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(InstallerConfiguration));
		using TextReader textReader = new StringReader(xml);
		return (InstallerConfiguration)xmlSerializer.Deserialize(textReader);
	}

	public override string ToString()
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(InstallerConfiguration));
		string empty = string.Empty;
		using StringWriter stringWriter = new StringWriter();
		using XmlWriter xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
		{
			OmitXmlDeclaration = true
		});
		XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[1] { XmlQualifiedName.Empty });
		xmlSerializer.Serialize(xmlWriter, this, namespaces);
		return stringWriter.ToString();
	}
}
