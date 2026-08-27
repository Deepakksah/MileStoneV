using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedComponent.Data;

public class PropertyBase : IXmlSerializable
{
	protected Guid _id;

	protected string _displayname;

	protected string _shortcut;

	protected PropertySortedList _properties = new PropertySortedList();

	protected string _itemName;

	public Guid Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return _displayname;
		}
		set
		{
			_displayname = value;
		}
	}

	public string Shortcut
	{
		get
		{
			return _shortcut;
		}
		set
		{
			_shortcut = value;
		}
	}

	public PropertySortedList Properties
	{
		get
		{
			return _properties;
		}
		set
		{
			_properties = value;
		}
	}

	public PropertyBase()
	{
	}

	public PropertyBase(string itemName)
	{
		_itemName = itemName;
	}

	public XmlSchema GetSchema()
	{
		throw new NotImplementedException();
	}

	public virtual void ReadXml(XmlReader reader)
	{
		if (!(reader.Name == _itemName))
		{
			return;
		}
		if (reader.MoveToAttribute("id"))
		{
			string text = reader.ReadContentAsString();
			if (!string.IsNullOrEmpty(text))
			{
				_id = new Guid(text);
			}
		}
		if (reader.MoveToAttribute("displayname"))
		{
			_displayname = reader.ReadContentAsString();
		}
		if (reader.MoveToAttribute("shortcut"))
		{
			_shortcut = reader.ReadContentAsString();
		}
	}

	public virtual void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString("id", _id.ToString());
		writer.WriteAttributeString("displayname", _displayname);
		writer.WriteAttributeString("shortcut", _shortcut);
	}

	public virtual void WriteXmlFragment(XmlWriter writer)
	{
		writer.WriteAttributeString("id", _id.ToString());
		writer.WriteAttributeString("displayname", _displayname);
		writer.WriteAttributeString("shortcut", _shortcut);
	}

	public void ReadProperty(XmlReader reader)
	{
		while (reader.Read())
		{
			if (!(reader.Name == "properties"))
			{
				continue;
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				break;
			}
			if (reader.IsEmptyElement)
			{
				continue;
			}
			XmlReader xmlReader = reader.ReadSubtree();
			bool flag = false;
			while (xmlReader.Read() && !flag)
			{
				if (xmlReader.Name == "property")
				{
					string text = "";
					string value = "";
					if (xmlReader.MoveToAttribute("name"))
					{
						text = xmlReader.ReadContentAsString();
					}
					if (xmlReader.MoveToAttribute("value"))
					{
						value = xmlReader.ReadContentAsString();
					}
					if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(value))
					{
						_properties.Add(text, value);
					}
				}
			}
		}
	}

	public void WriteProperty(XmlWriter writer)
	{
		writer.WriteStartElement("properties");
		for (int i = 0; i < _properties.Count; i++)
		{
			writer.WriteStartElement("property");
			writer.WriteAttributeString("name", _properties.GetKey(i).ToString());
			writer.WriteAttributeString("value", _properties.GetByIndex(i).ToString());
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}
}
