using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace VideoOS.Management.Common;

public class ServerAddresses : List<ServerAddress>
{
	public static string GetAsXml(ServerAddresses serverAddress)
	{
		if (serverAddress == null)
		{
			throw new ArgumentNullException("serverAddress");
		}
		return GetAsXml(serverAddress.ToArray());
	}

	public static string GetAsXml(ServerAddress[] serverAddress)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<Addresses></Addresses>");
		if (serverAddress != null)
		{
			foreach (ServerAddress serverAddress2 in serverAddress)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "Address", null);
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "url", null);
				xmlNode2.Value = serverAddress2.URL;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "type", null);
				xmlNode2.Value = serverAddress2.Type.ToString();
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static ServerAddresses GetFromXml(string serverAddressXml)
	{
		ServerAddresses serverAddresses = new ServerAddresses();
		try
		{
			if (!string.IsNullOrEmpty(serverAddressXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(serverAddressXml);
				XmlElement documentElement = xmlDocument.DocumentElement;
				if (documentElement.Name == "Addresses")
				{
					foreach (XmlNode childNode in documentElement.ChildNodes)
					{
						if (!(childNode.Name == "Address"))
						{
							continue;
						}
						XmlNode namedItem = childNode.Attributes.GetNamedItem("url");
						if (namedItem != null)
						{
							ServerAddress serverAddress = new ServerAddress();
							serverAddress.URL = namedItem.Value;
							namedItem = childNode.Attributes.GetNamedItem("type");
							if (namedItem != null && EnumIsDefined(typeof(ServerAddressType), namedItem.Value))
							{
								serverAddress.Type = (ServerAddressType)Enum.Parse(typeof(ServerAddressType), namedItem.Value, ignoreCase: true);
							}
							serverAddresses.Add(serverAddress);
						}
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
		return serverAddresses;
	}

	private static bool EnumIsDefined(Type enumType, string value)
	{
		if (!enumType.IsEnum)
		{
			throw new Exception("Input parameter 'enumType' does not represent an enum.");
		}
		if (new List<string>(Enum.GetNames(enumType)).Contains(value, StringComparer.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}
}
