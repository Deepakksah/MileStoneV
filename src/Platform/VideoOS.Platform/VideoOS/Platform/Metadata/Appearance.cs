using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Appearance : IXmlSerializable, IEquatable<Appearance>
{
	private static readonly object Lock = new object();

	private static DateTime _lastInvalidShape;

	public Transformation Transformation { get; set; }

	public Shape Shape { get; set; }

	public OnvifClass Class { get; set; }

	public DisplayText Description { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		BlankAllFields();
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void BlankAllFields()
	{
		Shape = null;
		Transformation = null;
		Class = null;
		Description = null;
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			if ((object)reader.LocalName == MetadataXml.ShapeElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				Shape shape = new Shape();
				shape.ReadXml(reader2);
				if (shape.IsValid)
				{
					Shape = shape;
				}
				else
				{
					lock (Lock)
					{
						if (DateTime.UtcNow - _lastInvalidShape > MetadataXml.LogIgnoreTimeSpand)
						{
							EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'Shape' is invalid and will be discarded. This message is logged at most once per minute", null);
							_lastInvalidShape = DateTime.UtcNow;
						}
					}
				}
			}
			if ((object)reader.LocalName == MetadataXml.TransformationElement)
			{
				using XmlReader reader3 = reader.ReadSubtree();
				Transformation transformation = new Transformation();
				transformation.ReadXml(reader3);
				Transformation = transformation;
			}
			if ((object)reader.LocalName == MetadataXml.ClassElement)
			{
				using XmlReader reader4 = reader.ReadSubtree();
				OnvifClass onvifClass = new OnvifClass();
				onvifClass.ReadXml(reader4);
				Class = onvifClass;
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
			if (reader.NodeType == XmlNodeType.Element && (object)MetadataXml.DescriptionElement == reader.LocalName && reader.Depth == 1)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				DisplayText displayText = new DisplayText();
				displayText.ReadXml(reader2);
				Description = displayText;
			}
		}
		while (reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		if (Transformation != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.TransformationElement, MetadataXml.OnvifNamespace);
			Transformation.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (Shape != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ShapeElement, MetadataXml.OnvifNamespace);
			Shape.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (Class != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ClassElement, MetadataXml.OnvifNamespace);
			Class.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (ExtensionDataPresent())
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ExtensionElement, MetadataXml.OnvifNamespace);
			if (Description != null)
			{
				writer.WriteStartElement(MetadataXml.DescriptionElement);
				Description.WriteXml(writer);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
		}
	}

	private bool ExtensionDataPresent()
	{
		return Description != null;
	}

	public bool Equals(Appearance other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (object.Equals(Transformation, other.Transformation))
		{
			return object.Equals(Shape, other.Shape);
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
		return Equals((Appearance)obj);
	}

	public override int GetHashCode()
	{
		return (((Transformation != null) ? Transformation.GetHashCode() : 0) * 397) ^ ((Shape != null) ? Shape.GetHashCode() : 0);
	}
}
