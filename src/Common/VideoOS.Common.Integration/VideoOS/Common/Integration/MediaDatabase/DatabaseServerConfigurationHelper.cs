using System;
using System.Xml;
using VideoOS.Common.Integration.MediaDatabase.PrivateConfigXmlExtensions;

namespace VideoOS.Common.Integration.MediaDatabase;

public static class DatabaseServerConfigurationHelper
{
	public static void InitializeServerLogging(XmlElement databaseServerElement, string logDirectory)
	{
		databaseServerElement.GetChildElement("logging").GetChildElement("path").InnerText = logDirectory;
	}

	public static void InitializeServerTablesDb(XmlElement databaseServer, string tablesDbDirectory)
	{
		databaseServer.GetChildElement("table_database").GetChildElement("path").InnerText = tablesDbDirectory;
	}

	public static void InitializeServerSocketCommunication(XmlElement databaseServerElement, string serverCertificateHash)
	{
		XmlElement childElement = databaseServerElement.GetChildElement("socket_communication");
		XmlNode xmlNode = childElement.SelectSingleNode("serverCertificateHash");
		if (xmlNode != null)
		{
			xmlNode.Value = serverCertificateHash;
		}
		else if (!string.IsNullOrEmpty(serverCertificateHash))
		{
			XmlElement xmlElement = childElement.OwnerDocument.CreateElement("serverCertificateHash");
			xmlElement.InnerText = serverCertificateHash;
			childElement.AppendChild(xmlElement);
		}
	}

	public static string GenerateSubXmlBuffer(XmlElement rootElement)
	{
		if (rootElement == null)
		{
			throw new ArgumentNullException("rootElement");
		}
		return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>" + rootElement.OuterXml;
	}
}
