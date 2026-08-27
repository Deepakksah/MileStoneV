using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Common;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ConfigurationItem
{
	public enum ValueTypes
	{
		Unknown,
		[Description("Integer")]
		Integer,
		[Description("Double")]
		Double,
		[Description("String")]
		String,
		[Description("Enum")]
		Enum
	}

	public const string SubItemsListName = "SubItems";

	public const string SubItemName = "SubItem";

	public const string IdName = "ID";

	public const string NameName = "Name";

	public const string ValueTypeName = "ValueType";

	public const string ValueInfosListName = "ValueInfos";

	public const string ValueInfoName = "ValueInfo";

	[JsonProperty("SubItems")]
	[XmlArray(ElementName = "SubItems")]
	[XmlArrayItem(ElementName = "SubItem")]
	public List<ConfigurationSubItem> SubItems;

	private Guid _id = Guid.Empty;

	private ValueTypes _type = ValueTypes.Integer;

	[JsonProperty("ValueInfos")]
	[XmlArray(ElementName = "ValueInfos")]
	[XmlArrayItem(ElementName = "ValueInfo")]
	public List<ValueInfo> ValueInfos;

	[JsonProperty("ID")]
	[XmlElement(ElementName = "ID")]
	public string ID
	{
		get
		{
			if (_id == Guid.Empty)
			{
				return null;
			}
			return _id.ToString();
		}
		set
		{
			try
			{
				_id = new Guid(value);
			}
			catch (Exception)
			{
				_id = Guid.Empty;
			}
		}
	}

	[JsonProperty("Name")]
	[XmlElement(ElementName = "Name")]
	public string Name { get; set; }

	[JsonProperty("ValueType")]
	[XmlElement(ElementName = "ValueType")]
	public string ValueType
	{
		get
		{
			return EnumsHelper.GetDescription(_type);
		}
		set
		{
			_type = EnumsHelper.EnumConverter<ValueTypes>(value);
		}
	}

	public ConfigurationItem()
	{
		SubItems = new List<ConfigurationSubItem>();
		ValueInfos = new List<ValueInfo>();
	}

	public void SetID(Guid id)
	{
		_id = id;
	}

	public Guid GetId()
	{
		return _id;
	}

	public void SetValueType(ValueTypes type)
	{
		_type = type;
	}

	public ValueTypes GetValueType()
	{
		return _type;
	}
}
