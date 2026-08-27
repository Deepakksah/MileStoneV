using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class ColorCluster : IXmlSerializable
{
	private Color _color;

	public Color Color => _color;

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
				if (reader.NamespaceURI.Equals(MetadataXml.OnvifNamespace))
				{
					string localName = reader.LocalName;
					if (MetadataXml.ColorElement == localName)
					{
						Color color = new Color();
						color.ReadXml(reader);
						_color = color;
					}
				}
				break;
			case XmlNodeType.EndElement:
				if (reader.LocalName == MetadataXml.ColorClusterElement)
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
