using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Transformation : IXmlSerializable, IEquatable<Transformation>
{
	private static readonly object Lock = new object();

	private static DateTime _lastScaleDiscarded;

	private static DateTime _lastTranslateDiscarded;

	public Vector Translate { get; set; }

	public Vector Scale { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		Scale = null;
		Translate = null;
		bool isEmptyElement = reader.IsEmptyElement;
		int depth = reader.Depth;
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
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
			if ((object)reader.LocalName == MetadataXml.ScaleElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				Vector vector = new Vector();
				vector.ReadXml(reader2);
				if (vector.AllAttributesWerePresent)
				{
					Scale = vector;
				}
				else
				{
					lock (Lock)
					{
						if (DateTime.UtcNow - _lastScaleDiscarded > MetadataXml.LogIgnoreTimeSpand)
						{
							EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'Scale' is incomplete and will be discarded", null);
							_lastScaleDiscarded = DateTime.UtcNow;
						}
					}
				}
			}
			if ((object)reader.LocalName != MetadataXml.TranslateElement)
			{
				continue;
			}
			using XmlReader reader3 = reader.ReadSubtree();
			Vector vector2 = new Vector();
			vector2.ReadXml(reader3);
			if (vector2.AllAttributesWerePresent)
			{
				Translate = vector2;
				continue;
			}
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastTranslateDiscarded > MetadataXml.LogIgnoreTimeSpand)
				{
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Element 'Translate' is incomplete and will be discarded", null);
					_lastTranslateDiscarded = DateTime.UtcNow;
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		if (Translate != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.TranslateElement, MetadataXml.OnvifNamespace);
			Translate.WriteXml(writer);
			writer.WriteEndElement();
		}
		if (Scale != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ScaleElement, MetadataXml.OnvifNamespace);
			Scale.WriteXml(writer);
			writer.WriteEndElement();
		}
	}

	public bool Equals(Transformation other)
	{
		if ((object)other == null)
		{
			return false;
		}
		if ((object)this == other)
		{
			return true;
		}
		if (object.Equals(Translate, other.Translate))
		{
			return object.Equals(Scale, other.Scale);
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
		return Equals((Transformation)obj);
	}

	public override int GetHashCode()
	{
		return (((Translate != null) ? Translate.GetHashCode() : 0) * 397) ^ ((Scale != null) ? Scale.GetHashCode() : 0);
	}

	public static bool operator ==(Transformation left, Transformation right)
	{
		return object.Equals(left, right);
	}

	public static bool operator !=(Transformation left, Transformation right)
	{
		return !object.Equals(left, right);
	}
}
