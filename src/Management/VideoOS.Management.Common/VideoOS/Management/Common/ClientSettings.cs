using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace VideoOS.Management.Common;

public sealed class ClientSettings : List<ClientSetting>
{
	public static string GetAsXml(ClientSettings clientSettings)
	{
		if (clientSettings == null)
		{
			throw new ArgumentNullException("clientSettings");
		}
		return GetAsXml(clientSettings.ToArray());
	}

	public static string GetAsXml(ClientSetting[] clientSettings)
	{
		if (clientSettings == null)
		{
			throw new ArgumentNullException("clientSettings");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<ClientSettings></ClientSettings>");
		if (clientSettings != null)
		{
			foreach (ClientSetting clientSetting in clientSettings)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "ClientSetting", null);
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "IDRole", null);
				xmlNode2.Value = clientSetting.RoleId.ToString();
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Attribute, "Live", null);
				xmlNode3.Value = (clientSetting.Live ? "1" : "0");
				xmlNode.Attributes.SetNamedItem(xmlNode3);
				XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Attribute, "Playback", null);
				xmlNode4.Value = (clientSetting.Playback ? "1" : "0");
				xmlNode.Attributes.SetNamedItem(xmlNode4);
				XmlNode xmlNode5 = xmlDocument.CreateNode(XmlNodeType.Attribute, "Setup", null);
				xmlNode5.Value = (clientSetting.Setup ? "1" : "0");
				xmlNode.Attributes.SetNamedItem(xmlNode5);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static ClientSettings GetFromXml(string clientSettingsXml)
	{
		if (clientSettingsXml == null)
		{
			throw new ArgumentNullException("clientSettingsXml");
		}
		ClientSettings clientSettings = new ClientSettings();
		if (!string.IsNullOrEmpty(clientSettingsXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(clientSettingsXml);
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (!(documentElement.Name == "ClientSettings"))
			{
				throw new XmlException("Root invalid");
			}
			foreach (XmlNode childNode in documentElement.ChildNodes)
			{
				if (childNode.Name == "ClientSetting")
				{
					ClientSetting clientSetting = new ClientSetting(new Guid(GetAttributeValue("IDRole", childNode)));
					string attributeValue = GetAttributeValue("Live", childNode);
					clientSetting.Live = GetValueAsBool(attributeValue);
					string attributeValue2 = GetAttributeValue("Playback", childNode);
					clientSetting.Playback = GetValueAsBool(attributeValue2);
					string attributeValue3 = GetAttributeValue("Setup", childNode);
					clientSetting.Setup = GetValueAsBool(attributeValue3);
					clientSettings.Add(clientSetting);
				}
			}
		}
		return clientSettings;
	}

	private static bool GetValueAsBool(string value)
	{
		if (string.IsNullOrEmpty(value))
		{
			return false;
		}
		if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) && result == 1)
		{
			return true;
		}
		return false;
	}

	private static string GetAttributeValue(string attributeName, XmlNode node)
	{
		return (node.Attributes.GetNamedItem(attributeName) ?? throw new XmlException(string.Format(CultureInfo.CurrentCulture, "Missing attribute {0}", attributeName))).Value;
	}
}
