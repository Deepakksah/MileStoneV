using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Management.Common;

public class ClientProfileSettings : KeyedCollection<string, ClientProfileSetting>
{
	public static string GetAsXml(ClientProfileSettings clientProfileSettings)
	{
		return GetAsXml(clientProfileSettings.ToArray());
	}

	public static string GetAsXml(ClientProfileSetting[] clientProfileSettings)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<Settings></Settings>");
		if (clientProfileSettings != null)
		{
			foreach (ClientProfileSetting clientProfileSetting in clientProfileSettings)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "Setting", null);
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "key", null);
				xmlNode2.Value = clientProfileSetting.Key;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "Value", null);
				xmlNode3.InnerText = clientProfileSetting.Value;
				xmlNode.AppendChild(xmlNode3);
				XmlNode xmlNode4 = xmlDocument.CreateNode(XmlNodeType.Element, "Locked", null);
				xmlNode4.InnerText = clientProfileSetting.Locked.ToString();
				xmlNode.AppendChild(xmlNode4);
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static ClientProfileSettings GetFromXml(string clientProfileSettingsXml)
	{
		ClientProfileSettings clientProfileSettings = new ClientProfileSettings();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(clientProfileSettingsXml);
		foreach (XPathNavigator item2 in xmlDocument.CreateNavigator().Select("//Settings/Setting"))
		{
			string attribute = item2.GetAttribute("key", string.Empty);
			string value = item2.SelectSingleNode("Value").Value;
			XPathNavigator xPathNavigator = item2.SelectSingleNode("Locked");
			bool result = true;
			if (!bool.TryParse(xPathNavigator.Value, out result))
			{
				result = true;
			}
			ClientProfileSetting item = new ClientProfileSetting(attribute, value, result);
			clientProfileSettings.Add(item);
		}
		return clientProfileSettings;
	}

	protected override string GetKeyForItem(ClientProfileSetting item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		return item.Key;
	}

	public bool TryGetValue(string key, out ClientProfileSetting clientProfileSetting)
	{
		if (base.Dictionary == null)
		{
			clientProfileSetting = null;
			return false;
		}
		return base.Dictionary.TryGetValue(key, out clientProfileSetting);
	}
}
