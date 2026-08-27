using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class DeviceConfiguration
{
	public string Name { get; set; }

	public string Id { get; set; }

	public bool Added { get; set; }

	public bool Enabled { get; set; }

	public string ItemPath => string.Format(CultureInfo.InvariantCulture, "{0}[{1}]", DeviceConfigurationFactory.GetItemType(DeviceConfigurationFactory.GetType(this)), Id);

	public void AppendXml(XmlDocument doc, XmlNode node)
	{
		if (doc == null)
		{
			throw new ArgumentNullException("doc");
		}
		if (node == null)
		{
			throw new ArgumentNullException("node");
		}
		XmlNode xmlNode = doc.CreateNode(XmlNodeType.Element, "device", null);
		XmlNode xmlNode2 = doc.CreateNode(XmlNodeType.Attribute, "type", null);
		xmlNode2.Value = DeviceConfigurationFactory.GetType(this);
		xmlNode.Attributes.SetNamedItem(xmlNode2);
		xmlNode2 = doc.CreateNode(XmlNodeType.Attribute, "name", null);
		xmlNode2.Value = Name;
		xmlNode.Attributes.SetNamedItem(xmlNode2);
		xmlNode2 = doc.CreateNode(XmlNodeType.Attribute, "id", null);
		xmlNode2.Value = Id;
		xmlNode.Attributes.SetNamedItem(xmlNode2);
		xmlNode2 = doc.CreateNode(XmlNodeType.Attribute, "added", null);
		xmlNode2.Value = Added.ToString();
		xmlNode.Attributes.SetNamedItem(xmlNode2);
		xmlNode2 = doc.CreateNode(XmlNodeType.Attribute, "enabled", null);
		xmlNode2.Value = Enabled.ToString();
		xmlNode.Attributes.SetNamedItem(xmlNode2);
		node.AppendChild(xmlNode);
	}

	public static DeviceConfiguration DeviceFromXml(XmlNode deviceNode)
	{
		if (deviceNode == null)
		{
			throw new ArgumentNullException("deviceNode");
		}
		DeviceConfiguration deviceConfiguration = null;
		XmlNode namedItem = deviceNode.Attributes.GetNamedItem("type");
		if (namedItem != null)
		{
			deviceConfiguration = DeviceConfigurationFactory.GetDevice(namedItem.Value);
		}
		namedItem = deviceNode.Attributes.GetNamedItem("id");
		if (namedItem != null)
		{
			deviceConfiguration.Id = namedItem.Value;
		}
		namedItem = deviceNode.Attributes.GetNamedItem("name");
		if (namedItem != null)
		{
			deviceConfiguration.Name = namedItem.Value;
		}
		namedItem = deviceNode.Attributes.GetNamedItem("added");
		if (namedItem != null)
		{
			deviceConfiguration.Added = bool.Parse(namedItem.Value);
		}
		namedItem = deviceNode.Attributes.GetNamedItem("enabled");
		if (namedItem != null)
		{
			deviceConfiguration.Enabled = bool.Parse(namedItem.Value);
		}
		return deviceConfiguration;
	}
}
