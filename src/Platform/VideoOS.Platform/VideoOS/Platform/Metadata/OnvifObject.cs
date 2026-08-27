using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class OnvifObject : IXmlSerializable, IEquatable<OnvifObject>
{
	public int ObjectId { get; set; }

	public Appearance Appearance { get; set; }

	public Behaviour Behaviour { get; set; }

	public OnvifObject()
		: this(0)
	{
	}

	public OnvifObject(int objectId)
	{
		ObjectId = objectId;
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		ObjectId = 0;
		Appearance = null;
		Behaviour = null;
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		ReadObjectIdValue(reader);
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void ReadObjectIdValue(XmlReader reader)
	{
		string attribute = reader.GetAttribute(MetadataXml.ObjectIdAttribute);
		if (!int.TryParse(attribute, MetadataXml.FloatStyle, MetadataXml.Culture, out var result) && EnvironmentManager.Instance != null)
		{
			EnvironmentManager.Instance.Log(error: false, "OnvifObject.ReadXml", "Value in ObjectId attribute is not an integer: " + attribute);
		}
		ObjectId = result;
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI != MetadataXml.OnvifNamespace || reader.Depth != rootDepth + 1 || reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			if ((object)reader.LocalName == MetadataXml.AppearanceElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				Appearance = new Appearance();
				Appearance.ReadXml(reader2);
			}
			if ((object)reader.LocalName == MetadataXml.BehaviourElement)
			{
				using XmlReader reader3 = reader.ReadSubtree();
				Behaviour = new Behaviour();
				Behaviour.ReadXml(reader3);
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString(MetadataXml.ObjectIdAttribute, ObjectId.ToString(CultureInfo.InvariantCulture));
		if (Appearance != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.AppearanceElement, MetadataXml.OnvifNamespace);
			Appearance.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (Behaviour != null && Behaviour.HasBehaviours)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.BehaviourElement, MetadataXml.OnvifNamespace);
			Behaviour.WriteXml(writer);
			writer.WriteEndElement();
		}
	}

	public bool Equals(OnvifObject other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (ObjectId == other.ObjectId && object.Equals(Appearance, other.Appearance))
		{
			return object.Equals(Behaviour, other.Behaviour);
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
		return Equals((OnvifObject)obj);
	}

	public override int GetHashCode()
	{
		return (((ObjectId * 397) ^ ((Appearance != null) ? Appearance.GetHashCode() : 0)) * 397) ^ ((Behaviour != null) ? Behaviour.GetHashCode() : 0);
	}

	public static bool operator ==(OnvifObject left, OnvifObject right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(OnvifObject left, OnvifObject right)
	{
		return !object.Equals(left, right);
	}
}
