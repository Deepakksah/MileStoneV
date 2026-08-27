using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Platform.Util;

public class EndpointDefinitions : List<EndpointDefinition>
{
	public static string GetAsXml(EndpointDefinitions serverEndpoints)
	{
		return GetAsXml(serverEndpoints.ToArray());
	}

	public static string GetAsXml(EndpointDefinition[] serverEndpoint)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<endpoints></endpoints>");
		if (serverEndpoint != null)
		{
			foreach (EndpointDefinition endpointDefinition in serverEndpoint)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "endpoint", null);
				XmlElement xmlElement = xmlDocument.CreateElement("name");
				xmlElement.InnerText = endpointDefinition.Name;
				xmlNode.AppendChild(xmlElement);
				XmlNode xmlNode2 = xmlDocument.CreateElement("description");
				xmlNode2.InnerText = endpointDefinition.Description;
				xmlNode.AppendChild(xmlNode2);
				XmlNode xmlNode3 = xmlDocument.CreateElement("authenticationmode");
				xmlNode3.InnerText = endpointDefinition.AuthenticationType.ToString();
				xmlNode.AppendChild(xmlNode3);
				XmlNode xmlNode4 = xmlDocument.CreateElement("servicepath");
				xmlNode4.InnerText = endpointDefinition.ServicePath;
				xmlNode.AppendChild(xmlNode4);
				XmlNode xmlNode5 = xmlDocument.CreateElement("protocol");
				xmlNode5.InnerText = endpointDefinition.Protocol.ToString();
				xmlNode.AppendChild(xmlNode5);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static EndpointDefinitions GetFromXml(string serverEndpointXml)
	{
		EndpointDefinitions endpointDefinitions = new EndpointDefinitions();
		try
		{
			if (!string.IsNullOrEmpty(serverEndpointXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(serverEndpointXml);
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (documentElement.Name == "endpoints")
				{
					foreach (XmlNode childNode in documentElement.ChildNodes)
					{
						if (!(childNode.Name == "endpoint"))
						{
							continue;
						}
						EndpointDefinition endpointDefinition = new EndpointDefinition();
						foreach (XmlNode childNode2 in childNode.ChildNodes)
						{
							if (childNode2.Name == "name")
							{
								endpointDefinition.Name = childNode2.InnerText;
							}
							if (childNode2.Name == "description")
							{
								endpointDefinition.Description = childNode2.InnerText;
							}
							if (childNode2.Name == "authenticationmode")
							{
								endpointDefinition.AuthenticationType = childNode2.InnerText;
							}
							if (childNode2.Name == "servicepath")
							{
								endpointDefinition.ServicePath = childNode2.InnerText;
							}
							if (childNode2.Name == "protocol")
							{
								endpointDefinition.Protocol = childNode2.InnerText;
							}
						}
						endpointDefinitions.Add(endpointDefinition);
					}
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return endpointDefinitions;
	}
}
