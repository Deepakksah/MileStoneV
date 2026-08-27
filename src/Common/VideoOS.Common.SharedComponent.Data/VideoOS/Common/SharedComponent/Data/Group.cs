using System;
using System.Collections.Generic;
using System.Xml;

namespace VideoOS.Common.SharedComponent.Data;

public class Group : PropertyBase
{
	private List<Group> _groups;

	private List<View> _views;

	private Guid _smartClientId = Guid.Empty;

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

	public List<View> Views
	{
		get
		{
			return _views;
		}
		set
		{
			_views = value;
		}
	}

	public Guid SmartClientId
	{
		set
		{
			_smartClientId = value;
		}
	}

	public Group()
		: base("group")
	{
		_groups = new List<Group>();
		_views = new List<View>();
	}

	public override void ReadXml(XmlReader reader)
	{
		while (reader.Read())
		{
			switch (reader.Name)
			{
			case "group":
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					return;
				}
				base.ReadXml(reader);
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
				break;
			case "groups":
			{
				if (reader.IsEmptyElement)
				{
					break;
				}
				XmlReader xmlReader2 = reader.ReadSubtree();
				bool flag2 = false;
				while (xmlReader2.Read() && !flag2)
				{
					string name = xmlReader2.Name;
					if (!(name == "group"))
					{
						if (name == "groups" && xmlReader2.NodeType == XmlNodeType.EndElement)
						{
							flag2 = true;
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
			case "views":
			{
				if (reader.IsEmptyElement)
				{
					break;
				}
				XmlReader xmlReader = reader.ReadSubtree();
				bool flag = false;
				while (xmlReader.Read() && !flag)
				{
					string name = xmlReader.Name;
					if (!(name == "view"))
					{
						if (name == "views" && xmlReader.NodeType == XmlNodeType.EndElement)
						{
							flag = true;
						}
					}
					else
					{
						View view = new View();
						view.ReadXml(xmlReader.ReadSubtree());
						Views.Add(view);
					}
				}
				break;
			}
			case "properties":
				ReadProperty(reader.ReadSubtree());
				break;
			}
		}
	}

	public override void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("group");
		base.WriteXml(writer);
		if (_smartClientId != Guid.Empty)
		{
			writer.WriteAttributeString("smartClientId", _smartClientId.ToString());
		}
		writer.WriteStartElement("groups");
		foreach (Group group in Groups)
		{
			group.WriteXml(writer);
		}
		writer.WriteEndElement();
		writer.WriteStartElement("views");
		foreach (View view in Views)
		{
			view.WriteXml(writer);
		}
		writer.WriteEndElement();
		WriteProperty(writer);
		writer.WriteEndElement();
	}

	public override void WriteXmlFragment(XmlWriter writer)
	{
		writer.WriteStartElement("group");
		base.WriteXmlFragment(writer);
		if (_smartClientId != Guid.Empty)
		{
			writer.WriteAttributeString("smartClientId", _smartClientId.ToString());
		}
		WriteProperty(writer);
		writer.WriteEndElement();
	}
}
