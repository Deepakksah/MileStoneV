using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Platform;

namespace VideoOS.LPR.Common.Config;

public sealed class VideoSourceItem : Item
{
	private VideoSourceItemConfig _config;

	public IVideoSourceConfig Config => _config;

	public override string Name
	{
		get
		{
			return _config.Name;
		}
		set
		{
			_config.Name = value;
		}
	}

	public override FQID FQID
	{
		get
		{
			return _config.FQID;
		}
		set
		{
			_config.FQID = value;
		}
	}

	public override bool Enabled
	{
		get
		{
			return _config.Enabled;
		}
		set
		{
			_config.Enabled = value;
		}
	}

	public string Description
	{
		get
		{
			return _config.Description;
		}
		set
		{
			_config.Description = value;
		}
	}

	public VideoSourceItem(Item item)
		: this(item, updateCameraName: true)
	{
	}

	private VideoSourceItem(Item item, bool updateCameraName)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		foreach (KeyValuePair<string, string> property in item.Properties)
		{
			Properties.Add(property.Key, property.Value);
		}
		_config = new VideoSourceItemConfig(item.FQID.Clone(), item.Name, Properties);
		if (updateCameraName)
		{
			UpdateCameraName(_config.CameraFQID);
		}
	}

	private void UpdateCameraName(FQID cameraFqid)
	{
		Configuration instance = Configuration.Instance;
		if (instance == null)
		{
			return;
		}
		try
		{
			Item item = instance.GetItem(cameraFqid);
			if (item != null)
			{
				_config.Name = item.Name;
			}
		}
		catch (MIPException)
		{
		}
	}

	public VideoSourceItem Clone()
	{
		return new VideoSourceItem(this, updateCameraName: false);
	}

	public void ApplyConfiguration(Item item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		Properties.Clear();
		foreach (KeyValuePair<string, string> property in item.Properties)
		{
			Properties.Add(property.Key, property.Value);
		}
		_config = new VideoSourceItemConfig(item.FQID.Clone(), item.Name, Properties);
	}

	public string SerializeItem()
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			Encoding = new UnicodeEncoding()
		}))
		{
			xmlWriter.WriteStartElement("Item");
			xmlWriter.WriteStartElement("Name");
			xmlWriter.WriteString(Name);
			xmlWriter.WriteEndElement();
			FQID.Serialize(xmlWriter);
			xmlWriter.WriteStartElement("Properties");
			foreach (KeyValuePair<string, string> property in Properties)
			{
				xmlWriter.WriteStartElement("Property");
				xmlWriter.WriteAttributeString("Key", property.Key);
				xmlWriter.WriteAttributeString("Value", property.Value);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
			xmlWriter.WriteEndElement();
			xmlWriter.Flush();
		}
		return stringBuilder.ToString();
	}

	public static VideoSourceItem DeserializeItem(string itemXml)
	{
		Item item = null;
		if (!string.IsNullOrEmpty(itemXml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			XmlReader reader = XmlReader.Create(new StringReader(itemXml), new XmlReaderSettings
			{
				XmlResolver = null
			});
			xmlDocument.Load(reader);
			item = new Item();
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Item/Name");
			item.Name = xmlNode.InnerXml;
			XmlNode xmlNode2 = xmlDocument.SelectSingleNode("Item/FQID");
			item.FQID = new FQID(xmlNode2.OuterXml);
			foreach (XmlNode item2 in xmlDocument.SelectNodes("Item/Properties/Property"))
			{
				string value = item2.Attributes["Key"].Value;
				if (!item.Properties.ContainsKey(value))
				{
					item.Properties.Add(value, item2.Attributes["Value"].Value);
				}
			}
		}
		if (item != null && item.FQID != null)
		{
			return new VideoSourceItem(item);
		}
		return null;
	}
}
