using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedComponent.Data;

public class ViewGroup : IXmlSerializable
{
	private string _xmlns;

	protected string _properties;

	private List<Group> _groups;

	public string XmlNamespace
	{
		get
		{
			return _xmlns;
		}
		set
		{
			_xmlns = value;
		}
	}

	public string Properties
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

	public List<Group> Groups
	{
		get
		{
			return _groups;
		}
		set
		{
			_groups = value;
		}
	}

	public ViewGroup()
	{
		_groups = new List<Group>();
		_xmlns = "";
	}

	public void ReadXml(XmlReader reader)
	{
		while (reader.Read())
		{
			switch (reader.Name)
			{
			case "viewgroup":
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					return;
				}
				if (reader.MoveToAttribute("xmlns"))
				{
					_xmlns = reader.ReadContentAsString();
				}
				break;
			case "groups":
			{
				if (reader.IsEmptyElement)
				{
					break;
				}
				XmlReader xmlReader2 = reader.ReadSubtree();
				bool flag = false;
				while (xmlReader2.Read() && !flag)
				{
					string name = xmlReader2.Name;
					if (!(name == "group"))
					{
						if (name == "groups" && xmlReader2.NodeType == XmlNodeType.EndElement)
						{
							flag = true;
						}
					}
					else
					{
						Group obj = new Group();
						obj.ReadXml(xmlReader2.ReadSubtree());
						Groups.Add(obj);
					}
				}
				break;
			}
			case "properties":
			{
				XmlReader xmlReader = reader.ReadSubtree();
				if (xmlReader.Read())
				{
					_properties = xmlReader.ReadInnerXml();
				}
				break;
			}
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("viewgroup", _xmlns);
		writer.WriteAttributeString("xmlns", _xmlns);
		writer.WriteStartElement("groups", _xmlns);
		foreach (Group group in Groups)
		{
			group.WriteXml(writer);
		}
		writer.WriteEndElement();
		if (!string.IsNullOrEmpty(_properties))
		{
			writer.WriteRaw(_properties);
		}
		else
		{
			writer.WriteStartElement("properties", _xmlns);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	public XmlSchema GetSchema()
	{
		throw new NotImplementedException();
	}

	public void WriteXmlFragment(XmlWriter writer)
	{
		writer.WriteStartElement("viewgroup", _xmlns);
		writer.WriteAttributeString("xmlns", _xmlns);
		if (!string.IsNullOrEmpty(_properties))
		{
			writer.WriteRaw(_properties);
		}
		else
		{
			writer.WriteStartElement("properties", _xmlns);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}
}
