using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.ConfigurationItems;

namespace VideoOS.Platform.Admin;

public class AssociatedProperties
{
	private Dictionary<string, string> _properties = new Dictionary<string, string>();

	private CustomProperties _customProperties;

	private string _propertyId;

	public Dictionary<string, string> Properties => _properties;

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(_customProperties.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public void Save()
	{
		_customProperties.SetProperty(_propertyId, PackProperties());
		_customProperties.Save();
	}

	public AssociatedProperties(Item item, Guid pluginId)
	{
		if (item == null || item.FQID == null)
		{
			throw new ArgumentNullException("item");
		}
		_propertyId = pluginId.ToString();
		string path = Factory.CreatePath(item.FQID);
		ServerId serverId = ((item.FQID.ServerId.ServerType == "XPCORS") ? item.FQID.GetParent().GetParent().ServerId : item.FQID.ServerId);
		ConfigurationItem item2 = ConfigurationService.GetItem(serverId, path);
		ConfigurationItem configurationItem = null;
		if (!item2.ChildrenFilled)
		{
			item2.Children = ConfigurationService.GetChildren(serverId, item2.Path);
		}
		ConfigurationItem[] children = item2.Children;
		foreach (ConfigurationItem configurationItem2 in children)
		{
			if (configurationItem2.ItemType == "CustomPropertiesFolder")
			{
				configurationItem = configurationItem2;
				break;
			}
		}
		bool flag = false;
		if (configurationItem != null)
		{
			if (!configurationItem.ChildrenFilled)
			{
				configurationItem.Children = ConfigurationService.GetChildren(serverId, configurationItem.Path);
			}
			if (configurationItem.Children != null && configurationItem.Children.Length != 0)
			{
				_customProperties = new CustomProperties(serverId, configurationItem.Children[0]);
				if (_customProperties != null)
				{
					string property = _customProperties.GetProperty(_propertyId);
					if (property != null)
					{
						UnpackProperties(property);
					}
					flag = true;
				}
			}
		}
		if (!flag)
		{
			throw new MIPException("Server does not support custom properties");
		}
	}

	private void UnpackProperties(string xml)
	{
		MemoryStream memoryStream = null;
		try
		{
			memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
			memoryStream.Flush();
			memoryStream.Position = 0L;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(memoryStream);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("Top/p");
			if (xmlNodeList == null)
			{
				return;
			}
			foreach (XmlNode item in xmlNodeList)
			{
				XmlNode xmlNode = item.SelectSingleNode("Name");
				XmlNode xmlNode2 = item.SelectSingleNode("Value");
				if (xmlNode != null && xmlNode2 != null)
				{
					_properties.Add(xmlNode.InnerText, xmlNode2.InnerText);
				}
			}
		}
		catch (XmlException ex)
		{
			EnvironmentManager.Instance.Log(error: true, "UserControlMIPTab", ex.Message);
		}
		finally
		{
			memoryStream?.Dispose();
		}
	}

	private string PackProperties()
	{
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.UTF8;
		string empty = string.Empty;
		using MemoryStream memoryStream = new MemoryStream();
		using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
		{
			xmlWriter.WriteStartDocument();
			xmlWriter.WriteStartElement("Top");
			foreach (string key in _properties.Keys)
			{
				xmlWriter.WriteStartElement("p");
				xmlWriter.WriteElementString("Name", key);
				xmlWriter.WriteElementString("Value", _properties[key]);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndDocument();
		}
		return Encoding.UTF8.GetString(memoryStream.ToArray());
	}
}
