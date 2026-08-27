using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Behaviour : IXmlSerializable, IEquatable<Behaviour>
{
	public bool HasBehaviours
	{
		get
		{
			if (!IsIdle)
			{
				return IsRemoved;
			}
			return true;
		}
	}

	public bool IsIdle { get; set; }

	public bool IsRemoved { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		IsIdle = false;
		IsRemoved = false;
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadBehaviourChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void ReadBehaviourChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if ((object)reader.NamespaceURI == MetadataXml.OnvifNamespace && reader.Depth == rootDepth + 1 && reader.NodeType == XmlNodeType.Element)
			{
				if ((object)reader.LocalName == MetadataXml.BehaviourRemovedElement)
				{
					IsRemoved = true;
				}
				if ((object)reader.LocalName == MetadataXml.BehaviourIdleElement)
				{
					IsIdle = true;
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		if (IsRemoved)
		{
			writer.WriteElementString(MetadataXml.OnvifPrefix, MetadataXml.BehaviourRemovedElement, MetadataXml.OnvifNamespace, string.Empty);
		}
		if (IsIdle)
		{
			writer.WriteElementString(MetadataXml.OnvifPrefix, MetadataXml.BehaviourIdleElement, MetadataXml.OnvifNamespace, string.Empty);
		}
	}

	public bool Equals(Behaviour other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (IsIdle.Equals(other.IsIdle))
		{
			return IsRemoved.Equals(other.IsRemoved);
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
		return Equals((Behaviour)obj);
	}

	public override int GetHashCode()
	{
		return (IsIdle.GetHashCode() * 397) ^ IsRemoved.GetHashCode();
	}
}
