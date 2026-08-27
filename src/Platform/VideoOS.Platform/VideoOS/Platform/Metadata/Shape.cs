using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Shape : IXmlSerializable, IEquatable<Shape>
{
	private static readonly object Lock = new object();

	private static DateTime _lastBoundingBoxNotReadLog;

	private static DateTime _lastBoundingBoxDiscarded;

	private static DateTime _lastCenterOfGravityDiscarded;

	public Rectangle BoundingBox { get; set; }

	public Vector CenterOfGravity { get; set; }

	public bool IsValid => BoundingBox != null;

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		BoundingBox = null;
		CenterOfGravity = null;
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
		lock (Lock)
		{
			if (BoundingBox == null && DateTime.UtcNow - _lastBoundingBoxNotReadLog > MetadataXml.LogIgnoreTimeSpand)
			{
				EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'BoundingBox' could not be read", null);
				_lastBoundingBoxNotReadLog = DateTime.UtcNow;
			}
		}
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			if ((object)reader.LocalName == MetadataXml.BoundingBoxElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				Rectangle rectangle = new Rectangle();
				rectangle.ReadXml(reader2);
				if (rectangle.AllAttributesWerePresent)
				{
					BoundingBox = rectangle;
				}
				else
				{
					lock (Lock)
					{
						if (DateTime.UtcNow - _lastBoundingBoxDiscarded > MetadataXml.LogIgnoreTimeSpand)
						{
							EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'BoundingBox' is incomplete and will be discarded", null);
							_lastBoundingBoxDiscarded = DateTime.UtcNow;
						}
					}
				}
			}
			if ((object)reader.LocalName == MetadataXml.CenterOfGravityElement)
			{
				using XmlReader reader3 = reader.ReadSubtree();
				Vector vector = new Vector();
				vector.ReadXml(reader3);
				if (vector.AllAttributesWerePresent)
				{
					CenterOfGravity = vector;
				}
				else
				{
					lock (Lock)
					{
						if (DateTime.UtcNow - _lastCenterOfGravityDiscarded > MetadataXml.LogIgnoreTimeSpand)
						{
							EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'CenterOfGravity' is incomplete and will be discarded", null);
							_lastCenterOfGravityDiscarded = DateTime.UtcNow;
						}
					}
				}
			}
			if ((object)reader.LocalName == MetadataXml.ExtensionElement)
			{
				ReadExtensionData(reader.ReadSubtree());
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	private void ReadExtensionData(XmlReader reader)
	{
		do
		{
			if (reader.NodeType != XmlNodeType.Element || (object)MetadataXml.BoundingBoxAppearanceElement != reader.LocalName)
			{
				continue;
			}
			using XmlReader reader2 = reader.ReadSubtree();
			if (BoundingBox != null)
			{
				BoundingBox.ReadAppearanceExtensionXml(reader2);
			}
		}
		while (reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		if (BoundingBox != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.BoundingBoxElement, MetadataXml.OnvifNamespace);
			BoundingBox.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (CenterOfGravity != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.CenterOfGravityElement, MetadataXml.OnvifNamespace);
			CenterOfGravity.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (BoundingBox != null && BoundingBox.HasExtensionDataPresent())
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ExtensionElement, MetadataXml.OnvifNamespace);
			writer.WriteStartElement(MetadataXml.BoundingBoxAppearanceElement);
			BoundingBox.WriteAppearanceExtenstionXml(writer);
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
	}

	public bool Equals(Shape other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(BoundingBox, other.BoundingBox))
		{
			return object.Equals(CenterOfGravity, other.CenterOfGravity);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((Shape)obj);
	}

	public override int GetHashCode()
	{
		return (((BoundingBox != null) ? BoundingBox.GetHashCode() : 0) * 397) ^ ((CenterOfGravity != null) ? CenterOfGravity.GetHashCode() : 0);
	}
}
