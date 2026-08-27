using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Management.Server.ConfigurationApi;

public class EdgeRetrievalMetadata
{
	private const string RootNodeName = "RetrievalMetadata";

	private const string UserNameAttributeName = "UserName";

	private const string CreatedAttributeName = "Created";

	private const string IdentifierElementName = "Identifier";

	private const string RelatedTasksNodeName = "RelatedTasks";

	public static string Create(string userName, Guid identifier, IEnumerable<Guid> relatedTasks)
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("RetrievalMetadata");
		xmlElement.SetAttribute("UserName", userName);
		xmlElement.SetAttribute("Created", XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc));
		xmlElement.SetAttribute("Identifier", identifier.ToString());
		XmlElement xmlElement2 = xmlDocument.CreateElement("RelatedTasks");
		foreach (Guid relatedTask in relatedTasks)
		{
			XmlElement xmlElement3 = xmlDocument.CreateElement("Identifier");
			xmlElement3.InnerText = relatedTask.ToString();
			xmlElement2.AppendChild(xmlElement3);
		}
		xmlElement.AppendChild(xmlElement2);
		xmlDocument.AppendChild(xmlElement);
		return xmlDocument.OuterXml;
	}

	public static bool Read(string xml, out string userName, out DateTime creationTime, out Guid identifier, out IEnumerable<Guid> relatedTasks)
	{
		if (string.IsNullOrWhiteSpace(xml))
		{
			userName = "";
			creationTime = DateTime.MinValue;
			identifier = Guid.Empty;
			relatedTasks = null;
			return false;
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		if (!(xmlDocument.SelectSingleNode("RetrievalMetadata") is XmlElement xmlElement))
		{
			throw new ArgumentException("Invalid metadata format");
		}
		userName = xmlElement.GetAttribute("UserName");
		creationTime = XmlConvert.ToDateTime(xmlElement.GetAttribute("Created"), XmlDateTimeSerializationMode.Utc);
		identifier = new Guid(xmlElement.GetAttribute("Identifier"));
		List<Guid> list = new List<Guid>();
		XmlNode xmlNode = xmlElement.SelectSingleNode("RelatedTasks");
		if (xmlNode != null)
		{
			foreach (object childNode in xmlNode.ChildNodes)
			{
				list.Add(new Guid(((XmlElement)childNode).InnerText));
			}
		}
		relatedTasks = list;
		return true;
	}
}
