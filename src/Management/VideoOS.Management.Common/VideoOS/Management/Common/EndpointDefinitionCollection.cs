using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace VideoOS.Management.Common;

public class EndpointDefinitionCollection : ReadOnlyCollection<EndpointDefinition>
{
	public EndpointDefinitionCollection(IList<EndpointDefinition> list)
		: base(list)
	{
	}

	public static string GetAsXml(EndpointDefinitionCollection serverEndpoints)
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
				XmlNode xmlNode5 = xmlDocument.CreateElement("scheme");
				xmlNode5.InnerText = endpointDefinition.Scheme.ToString();
				xmlNode.AppendChild(xmlNode5);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static EndpointDefinitionCollection GetFromXml(string serverEndpointXml)
	{
		List<EndpointDefinition> list = new List<EndpointDefinition>();
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
							if (childNode2.Name == "authenticationmode" && EnumIsDefined(typeof(AuthenticationType), childNode2.InnerText))
							{
								endpointDefinition.AuthenticationType = (AuthenticationType)Enum.Parse(typeof(AuthenticationType), childNode2.InnerText, ignoreCase: true);
							}
							if (childNode2.Name == "servicepath")
							{
								endpointDefinition.ServicePath = childNode2.InnerText;
							}
							if (childNode2.Name == "scheme")
							{
								endpointDefinition.Scheme = childNode2.InnerText;
							}
						}
						list.Add(endpointDefinition);
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
		return new EndpointDefinitionCollection(list);
	}

	private static bool EnumIsDefined(Type enumType, string value)
	{
		if (!enumType.IsEnum)
		{
			throw new ArgumentException("Input parameter 'enumType' does not represent an enum.");
		}
		if (new List<string>(Enum.GetNames(enumType)).Contains(value, StringComparer.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}
}
