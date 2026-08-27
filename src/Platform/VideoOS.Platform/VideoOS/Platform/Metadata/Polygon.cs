using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Polygon : IXmlSerializable
{
	private readonly List<Vector> _points = new List<Vector>();

	public List<Vector> Points => _points;

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
					if (MetadataXml.PointElement == localName)
					{
						Vector vector = new Vector();
						vector.ReadXml(reader.ReadSubtree());
						_points.Add(vector);
					}
				}
				break;
			case XmlNodeType.EndElement:
				if (reader.LocalName == MetadataXml.PolygonElement)
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
