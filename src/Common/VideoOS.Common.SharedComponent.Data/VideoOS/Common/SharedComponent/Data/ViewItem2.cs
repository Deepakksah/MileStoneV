using System;
using System.Xml;

namespace VideoOS.Common.SharedComponent.Data;

public class ViewItem2 : PropertyBase
{
	private string _type;

	private string _viewiteminfo;

	private Guid _smartClientId = Guid.Empty;

	public string Type
	{
		get
		{
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public string ViewItemInfo
	{
		get
		{
			return _viewiteminfo;
		}
		set
		{
			_viewiteminfo = value;
		}
	}

	public Guid SmartClientId
	{
		set
		{
			_smartClientId = value;
		}
	}

	public ViewItem2()
		: base("viewitem")
	{
	}

	public override void ReadXml(XmlReader reader)
	{
		while (reader.Read())
		{
			string name = reader.Name;
			if (!(name == "viewitem"))
			{
				if (name == "properties")
				{
					ReadProperty(reader.ReadSubtree());
				}
				else if (reader.NodeType == XmlNodeType.Element)
				{
					XmlReader xmlReader = reader.ReadSubtree();
					if (xmlReader.Read())
					{
						_viewiteminfo = xmlReader.ReadOuterXml();
					}
				}
				continue;
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				break;
			}
			base.ReadXml(reader);
			if (reader.MoveToAttribute("type"))
			{
				_type = reader.ReadContentAsString();
			}
			if (reader.MoveToAttribute("smartClientId"))
			{
				try
				{
					_smartClientId = new Guid(reader.ReadContentAsString());
				}
				catch (ArgumentNullException)
				{
					_smartClientId = Guid.Empty;
				}
				catch (OverflowException)
				{
					_smartClientId = Guid.Empty;
				}
				catch (FormatException)
				{
					_smartClientId = Guid.Empty;
				}
			}
		}
	}

	public override void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("viewitem");
		base.WriteXml(writer);
		writer.WriteAttributeString("type", _type);
		if (_smartClientId != Guid.Empty)
		{
			writer.WriteAttributeString("smartClientId", _smartClientId.ToString());
		}
		if (!string.IsNullOrEmpty(_viewiteminfo))
		{
			writer.WriteRaw(_viewiteminfo);
		}
		WriteProperty(writer);
		writer.WriteEndElement();
	}

	public override void WriteXmlFragment(XmlWriter writer)
	{
		writer.WriteStartElement("viewitem");
		base.WriteXmlFragment(writer);
		writer.WriteAttributeString("type", _type);
		if (_smartClientId != Guid.Empty)
		{
			writer.WriteAttributeString("smartClientId", _smartClientId.ToString());
		}
		if (!string.IsNullOrEmpty(_viewiteminfo))
		{
			writer.WriteRaw(_viewiteminfo);
		}
		WriteProperty(writer);
		writer.WriteEndElement();
	}
}
