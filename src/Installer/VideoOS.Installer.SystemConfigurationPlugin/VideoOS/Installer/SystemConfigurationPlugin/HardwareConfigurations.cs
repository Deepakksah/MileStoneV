using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareConfigurations : Collection<HardwareConfiguration>
{
	public static string GetAsXml(HardwareConfigurations hardwareConfigurations)
	{
		if (hardwareConfigurations == null)
		{
			throw new ArgumentNullException("hardwareConfigurations");
		}
		return GetAsXml(hardwareConfigurations.ToArray());
	}

	public static string GetAsXml(HardwareConfiguration[] hardwareConfigurations)
	{
		if (hardwareConfigurations == null)
		{
			throw new ArgumentNullException("hardwareConfigurations");
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml("<CameraConfigurations></CameraConfigurations>");
		if (hardwareConfigurations != null)
		{
			foreach (HardwareConfiguration hardwareConfiguration in hardwareConfigurations)
			{
				XmlNode xmlNode = xmlDocument.CreateNode(XmlNodeType.Element, "CameraConfiguration", null);
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "userName", null);
				xmlNode2.Value = hardwareConfiguration.UserName;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "password", null);
				xmlNode2.Value = hardwareConfiguration.Password;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "driverPath", null);
				xmlNode2.Value = hardwareConfiguration.DriverPath;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "ipAddress", null);
				xmlNode2.Value = hardwareConfiguration.IPAddress;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "validated", null);
				xmlNode2.Value = hardwareConfiguration.Validated.ToString();
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "cameraName", null);
				xmlNode2.Value = hardwareConfiguration.Name;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "id", null);
				xmlNode2.Value = hardwareConfiguration.Id;
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Attribute, "added", null);
				xmlNode2.Value = hardwareConfiguration.Added.ToString();
				xmlNode.Attributes.SetNamedItem(xmlNode2);
				if (hardwareConfiguration.Devices != null)
				{
					XmlNode xmlNode3 = xmlDocument.CreateNode(XmlNodeType.Element, "devices", null);
					foreach (DeviceConfiguration device in hardwareConfiguration.Devices)
					{
						device.AppendXml(xmlDocument, xmlNode3);
					}
					xmlNode.AppendChild(xmlNode3);
				}
				xmlDocument.DocumentElement.AppendChild(xmlNode);
			}
		}
		return xmlDocument.OuterXml;
	}

	public static HardwareConfigurations GetFromXml(string hardwareConfigurationsXml)
	{
		HardwareConfigurations hardwareConfigurations = new HardwareConfigurations();
		if (!string.IsNullOrEmpty(hardwareConfigurationsXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(hardwareConfigurationsXml);
			XmlElement documentElement = xmlDocument.DocumentElement;
			if (documentElement.Name == "CameraConfigurations")
			{
				foreach (XmlNode childNode in documentElement.ChildNodes)
				{
					if (!(childNode.Name == "CameraConfiguration"))
					{
						continue;
					}
					string userName = string.Empty;
					string password = string.Empty;
					string driverPath = null;
					string ipAddress = null;
					bool validated = false;
					string name = string.Empty;
					string id = string.Empty;
					bool added = false;
					List<DeviceConfiguration> list = new List<DeviceConfiguration>();
					XmlNode namedItem = childNode.Attributes.GetNamedItem("userName");
					if (namedItem != null)
					{
						userName = namedItem.Value;
						namedItem = childNode.Attributes.GetNamedItem("password");
						if (namedItem != null)
						{
							password = namedItem.Value;
						}
					}
					namedItem = childNode.Attributes.GetNamedItem("driverPath");
					if (namedItem != null)
					{
						driverPath = namedItem.Value;
					}
					namedItem = childNode.Attributes.GetNamedItem("ipAddress");
					if (namedItem != null)
					{
						ipAddress = namedItem.Value;
					}
					namedItem = childNode.Attributes.GetNamedItem("validated");
					if (namedItem != null)
					{
						validated = bool.Parse(namedItem.Value);
					}
					namedItem = childNode.Attributes.GetNamedItem("cameraName");
					if (namedItem != null)
					{
						name = namedItem.Value;
					}
					namedItem = childNode.Attributes.GetNamedItem("id");
					if (namedItem != null)
					{
						id = namedItem.Value;
					}
					namedItem = childNode.Attributes.GetNamedItem("added");
					if (namedItem != null)
					{
						added = bool.Parse(namedItem.Value);
					}
					XmlNode xmlNode2 = childNode.SelectSingleNode("devices");
					if (xmlNode2 != null)
					{
						XmlNodeList xmlNodeList = xmlNode2.SelectNodes("device");
						if (xmlNodeList != null)
						{
							foreach (XmlNode item2 in xmlNodeList)
							{
								list.Add(DeviceConfiguration.DeviceFromXml(item2));
							}
						}
					}
					HardwareConfiguration item = new HardwareConfiguration(userName, password, driverPath, ipAddress, validated, name)
					{
						Id = id,
						Added = added,
						Devices = list
					};
					hardwareConfigurations.Add(item);
				}
			}
		}
		return hardwareConfigurations;
	}

	public static bool IsEmpty(string cameraConfigurationsXml)
	{
		if (GetFromXml(cameraConfigurationsXml).Count == 0)
		{
			return true;
		}
		return false;
	}
}
