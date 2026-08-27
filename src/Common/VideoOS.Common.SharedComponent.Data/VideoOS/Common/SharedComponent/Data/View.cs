using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.SharedComponent.Data.ViewLayout;

namespace VideoOS.Common.SharedComponent.Data;

public class View : PropertyBase
{
	private const string LayoutPorpertyKey = "LayoutProperty";

	private string _viewlayouttype;

	private VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout _viewlayout;

	private List<ViewItem2> _viewitems;

	private KnownViewLayouts _knownViewLayouts;

	public string ViewLayoutType
	{
		get
		{
			return _viewlayouttype;
		}
		set
		{
			_viewlayouttype = value;
			_properties.Remove("LayoutProperty");
			UpdateProperties();
		}
	}

	public VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout ViewLayout
	{
		get
		{
			return _viewlayout;
		}
		set
		{
			_viewlayout = value;
		}
	}

	public List<ViewItem2> ViewItems
	{
		get
		{
			return _viewitems;
		}
		set
		{
			_viewitems = value;
		}
	}

	public View()
		: base("view")
	{
		_viewitems = new List<ViewItem2>();
	}

	public override void ReadXml(XmlReader reader)
	{
		while (reader.Read())
		{
			switch (reader.Name)
			{
			case "view":
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					UpdateProperties();
					return;
				}
				base.ReadXml(reader);
				if (reader.MoveToAttribute("viewlayouttype"))
				{
					_viewlayouttype = reader.ReadContentAsString();
				}
				break;
			case "viewitems":
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
					if (!(name == "viewitem"))
					{
						if (name == "viewitems" && xmlReader.NodeType == XmlNodeType.EndElement)
						{
							flag = true;
						}
					}
					else
					{
						ViewItem2 viewItem = new ViewItem2();
						viewItem.ReadXml(xmlReader.ReadSubtree());
						ViewItems.Add(viewItem);
					}
				}
				break;
			}
			case "properties":
				ReadProperty(reader.ReadSubtree());
				break;
			}
		}
		UpdateProperties();
	}

	public override void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("view");
		base.WriteXml(writer);
		writer.WriteAttributeString("viewlayouttype", _viewlayouttype);
		writer.WriteStartElement("viewitems");
		foreach (ViewItem2 viewItem in ViewItems)
		{
			viewItem.WriteXml(writer);
		}
		writer.WriteEndElement();
		WriteProperty(writer);
		writer.WriteEndElement();
	}

	public override void WriteXmlFragment(XmlWriter writer)
	{
		writer.WriteStartElement("view");
		base.WriteXmlFragment(writer);
		writer.WriteAttributeString("viewlayouttype", _viewlayouttype);
		WriteProperty(writer);
		writer.WriteEndElement();
	}

	private void UpdateProperties()
	{
		if (!_properties.Contains("LayoutProperty") && !string.IsNullOrEmpty(_viewlayouttype))
		{
			if (_knownViewLayouts == null)
			{
				_knownViewLayouts = new KnownViewLayouts();
			}
			if (_knownViewLayouts.TryGetValue(_viewlayouttype, out var value))
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlElement xmlElement = xmlDocument.CreateElement("ViewLayout");
				xmlDocument.AppendChild(xmlElement);
				XmlElement xmlElement2 = xmlDocument.CreateElement("Icon");
				xmlElement2.InnerText = Convert.ToBase64String(value.Layout.BinaryIcon);
				xmlElement.AppendChild(xmlElement2);
				XmlElement xmlElement3 = xmlDocument.CreateElement("ViewItems");
				xmlElement.AppendChild(xmlElement3);
				foreach (ViewItem viewItem in value.Layout.ViewItems)
				{
					XmlElement xmlElement4 = xmlDocument.CreateElement("ViewItem");
					xmlElement3.AppendChild(xmlElement4);
					XmlElement xmlElement5 = xmlDocument.CreateElement("Position");
					xmlElement4.AppendChild(xmlElement5);
					XmlElement xmlElement6 = xmlDocument.CreateElement("Size");
					xmlElement4.AppendChild(xmlElement6);
					XmlElement xmlElement7 = xmlDocument.CreateElement("X");
					xmlElement7.InnerText = viewItem.Position.X.ToString(CultureInfo.InvariantCulture);
					xmlElement5.AppendChild(xmlElement7);
					xmlElement7 = xmlDocument.CreateElement("Y");
					xmlElement7.InnerText = viewItem.Position.Y.ToString(CultureInfo.InvariantCulture);
					xmlElement5.AppendChild(xmlElement7);
					xmlElement7 = xmlDocument.CreateElement("Width");
					xmlElement7.InnerText = viewItem.Size.Width.ToString(CultureInfo.InvariantCulture);
					xmlElement6.AppendChild(xmlElement7);
					xmlElement7 = xmlDocument.CreateElement("Height");
					xmlElement7.InnerText = viewItem.Size.Height.ToString(CultureInfo.InvariantCulture);
					xmlElement6.AppendChild(xmlElement7);
				}
				if (_properties.ContainsKey("LayoutProperty"))
				{
					_properties["LayoutProperty"] = xmlDocument.OuterXml;
				}
				else
				{
					_properties.Add("LayoutProperty", xmlDocument.OuterXml);
				}
			}
		}
		if (_properties.Contains("LayoutProperty"))
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout));
			_viewlayout = (VideoOS.Common.SharedComponent.Data.ViewLayout.ViewLayout)xmlSerializer.Deserialize(new StringReader(_properties["LayoutProperty"].ToString()));
		}
	}
}
