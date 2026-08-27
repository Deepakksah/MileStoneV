using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Color : IXmlSerializable
{
	private float _x;

	private float _y;

	private float _z;

	private string _colorspace;

	public float X
	{
		get
		{
			return _x;
		}
		set
		{
			_x = value;
		}
	}

	public float Y
	{
		get
		{
			return _y;
		}
		set
		{
			_y = value;
		}
	}

	public float Z
	{
		get
		{
			return _z;
		}
		set
		{
			_z = value;
		}
	}

	public string Colorspace
	{
		get
		{
			return _colorspace;
		}
		set
		{
			_colorspace = value;
		}
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		do
		{
			switch (reader.NodeType)
			{
			case XmlNodeType.Element:
				if (!reader.NamespaceURI.Equals(MetadataXml.OnvifNamespace))
				{
					break;
				}
				while (reader.MoveToNextAttribute())
				{
					string localName = reader.LocalName;
					if (!(MetadataXml.ColorXAttribute == localName) && !(MetadataXml.ColorYAttribute == localName) && !(MetadataXml.ColorZAttribute == localName))
					{
						_ = MetadataXml.ColorspaceAttribute == localName;
					}
				}
				break;
			case XmlNodeType.EndElement:
				if (reader.LocalName == MetadataXml.ColorElement)
				{
					return;
				}
				break;
			}
		}
		while (reader.Read());
		reader.Close();
	}

	public void WriteXml(XmlWriter writer)
	{
		throw new NotImplementedException();
	}
}
