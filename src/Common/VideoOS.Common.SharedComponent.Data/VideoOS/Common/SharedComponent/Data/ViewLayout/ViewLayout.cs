using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Common.SharedComponent.Data.ViewLayout;

[Serializable]
public class ViewLayout
{
	private string _viewLayoutType;

	private Guid _id;

	private List<ViewItem> _viewItems;

	private Bitmap _icon;

	[XmlAttribute("ViewLayoutType", DataType = "string")]
	public string ViewLayoutType
	{
		get
		{
			return _viewLayoutType;
		}
		set
		{
			_viewLayoutType = value;
		}
	}

	[XmlAttribute("Id")]
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

	[XmlIgnore]
	public Bitmap Icon
	{
		get
		{
			return _icon;
		}
		set
		{
			_icon = value;
		}
	}

	[XmlElement("ViewLayoutIcon", typeof(byte[]), DataType = "base64Binary")]
	public byte[] BinaryIcon
	{
		get
		{
			return (byte[])TypeDescriptor.GetConverter(typeof(Bitmap)).ConvertTo(_icon, typeof(byte[]));
		}
		set
		{
			_icon = new Bitmap(new MemoryStream(value));
		}
	}

	public List<ViewItem> ViewItems
	{
		get
		{
			return _viewItems;
		}
		set
		{
			_viewItems = value;
		}
	}

	public bool IsSameLayout(ViewLayout otherViewLayout)
	{
		if (ViewLayoutType != otherViewLayout.ViewLayoutType)
		{
			return false;
		}
		if (ViewItems.Count != otherViewLayout.ViewItems.Count)
		{
			return false;
		}
		for (int i = 0; i < ViewItems.Count; i++)
		{
			if (ViewItems[i].Position != otherViewLayout.ViewItems[i].Position)
			{
				return false;
			}
			if (ViewItems[i].Size != otherViewLayout.ViewItems[i].Size)
			{
				return false;
			}
		}
		return true;
	}

	public string GetViewLayout()
	{
		XmlDocument xmlDocument = new XmlDocument();
		XmlElement xmlElement = xmlDocument.CreateElement("ViewLayout");
		xmlDocument.AppendChild(xmlElement);
		XmlElement xmlElement2 = xmlDocument.CreateElement("Icon");
		string innerText = "";
		if (Icon != null)
		{
			innerText = Convert.ToBase64String((byte[])new ImageConverter().ConvertTo(Icon, typeof(byte[])));
		}
		xmlElement2.InnerText = innerText;
		xmlElement.AppendChild(xmlElement2);
		XmlElement xmlElement3 = xmlDocument.CreateElement("ViewItems");
		xmlElement.AppendChild(xmlElement3);
		foreach (ViewItem viewItem in ViewItems)
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
		return SecurityElement.Escape(xmlDocument.OuterXml);
	}
}
