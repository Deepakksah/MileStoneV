using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class SerializableItem
{
	private SerializableItemTypes _itemType;

	private bool _isFolder;

	private string _type;

	[JsonProperty("Id")]
	[XmlAttribute("Id")]
	public Guid Id { get; set; }

	[JsonProperty("Name")]
	[XmlAttribute("Name")]
	public string Name { get; set; }

	[JsonProperty("Type")]
	[XmlAttribute("Type")]
	public string Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
			_itemType = EnumsHelper.EnumConverter<SerializableItemTypes>(Type);
		}
	}

	[XmlAttribute("Folder")]
	public string Folder
	{
		get
		{
			if (!_isFolder)
			{
				return null;
			}
			return "Yes";
		}
		set
		{
			_isFolder = string.Equals("Yes", value, StringComparison.CurrentCultureIgnoreCase);
		}
	}

	[XmlIgnore]
	public bool IsFolder
	{
		get
		{
			return _isFolder;
		}
		set
		{
			_isFolder = value;
		}
	}

	[JsonProperty("Properties")]
	[XmlElement("Properties")]
	public SerializableProperties<string, string, StringStringFactory> Properties { get; set; }

	[JsonProperty("Items")]
	[XmlArray("Items")]
	[XmlArrayItem("Item")]
	public List<SerializableItem> Children { get; set; }

	public SerializableItemTypes GetItemType()
	{
		return _itemType;
	}

	public void SetItemType(SerializableItemTypes itemType)
	{
		_itemType = itemType;
		_type = EnumsHelper.GetDescription(itemType);
	}

	public SerializableItem()
	{
		Children = new List<SerializableItem>();
		Properties = new SerializableProperties<string, string, StringStringFactory>();
	}

	public SerializableItem(SerializableItemTypes type)
	{
		Children = new List<SerializableItem>();
		Properties = new SerializableProperties<string, string, StringStringFactory>();
		_itemType = type;
		_type = EnumsHelper.GetDescription(type);
	}
}
