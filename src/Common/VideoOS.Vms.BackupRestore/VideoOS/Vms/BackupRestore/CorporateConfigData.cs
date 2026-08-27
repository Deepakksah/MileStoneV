using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Vms.BackupRestore;

public class CorporateConfigData
{
	private const string CorporateConfigDataXmlRootName = "CorporateConfigData";

	public string DBVersion { get; set; }

	public string ComputerName { get; set; }

	public CorporateConfigData(string dbVersion, string computerName)
	{
		DBVersion = dbVersion;
		ComputerName = computerName;
	}

	public static string WriteCorporateConfigDataAsXml(CorporateConfigData configData)
	{
		if (configData == null)
		{
			throw new ArgumentNullException("configData");
		}
		new XmlWriterSettings
		{
			OmitXmlDeclaration = false,
			Encoding = Encoding.Unicode
		};
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
		{
			xmlWriter.WriteStartElement("CorporateConfigData");
			xmlWriter.WriteStartElement("ComputerName");
			xmlWriter.WriteString(configData.ComputerName);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteStartElement("DBVersion");
			xmlWriter.WriteString(configData.DBVersion);
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
			xmlWriter.Close();
		}
		return stringBuilder.ToString();
	}

	public static CorporateConfigData ReadCorporateConfigDataFromXml(string xml)
	{
		CorporateConfigData corporateConfigData = null;
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		using StringReader input = new StringReader(xml);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreComments = true;
		xmlReaderSettings.IgnoreProcessingInstructions = true;
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.XmlResolver = null;
		xmlReaderSettings.ValidationType = ValidationType.Schema;
		using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
		xmlReader.MoveToContent();
		if (!string.Equals(xmlReader.Name, "CorporateConfigData", StringComparison.Ordinal))
		{
			return null;
		}
		xmlReader.Read();
		corporateConfigData = new CorporateConfigData(null, null);
		corporateConfigData.ComputerName = xmlReader.ReadInnerXml();
		corporateConfigData.DBVersion = xmlReader.ReadInnerXml();
		return corporateConfigData;
	}
}
