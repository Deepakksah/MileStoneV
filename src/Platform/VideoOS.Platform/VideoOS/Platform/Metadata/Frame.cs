using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Frame : IXmlSerializable, IEquatable<Frame>
{
	private static readonly object Lock = new object();

	private static DateTime _lastMissingTimestampLog;

	private readonly List<OnvifObject> _objects = new List<OnvifObject>();

	public Transformation Transformation { get; set; }

	public List<OnvifObject> Objects => _objects;

	public DateTime UtcTime { get; set; }

	public bool UtcTimeAttributeWasPresent { get; private set; }

	public Frame()
		: this(DateTime.MinValue)
	{
	}

	public Frame(DateTime utcTime)
	{
		UtcTime = utcTime;
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		int depth = reader.Depth;
		bool isEmptyElement = reader.IsEmptyElement;
		BlankAllFields();
		ReadUtcTime(reader);
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void BlankAllFields()
	{
		_objects.Clear();
		Transformation = null;
		UtcTime = DateTime.MinValue;
		UtcTimeAttributeWasPresent = false;
	}

	private void ReadUtcTime(XmlReader reader)
	{
		if (DateTime.TryParse(reader.GetAttribute(MetadataXml.UtcTimeAttribute), MetadataXml.Culture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out var result))
		{
			UtcTimeAttributeWasPresent = true;
			UtcTime = result;
			return;
		}
		lock (Lock)
		{
			if (!UtcTimeAttributeWasPresent && DateTime.UtcNow - _lastMissingTimestampLog > MetadataXml.LogIgnoreTimeSpand)
			{
				EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", "Required attribute 'UtcTime' could not be parsed or is missing", null);
				_lastMissingTimestampLog = DateTime.UtcNow;
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
			if ((object)reader.LocalName == MetadataXml.TransformationElement)
			{
				using XmlReader reader2 = reader.ReadSubtree();
				Transformation = new Transformation();
				Transformation.ReadXml(reader2);
			}
			else if ((object)reader.LocalName == MetadataXml.ObjectElement)
			{
				using XmlReader reader3 = reader.ReadSubtree();
				OnvifObject onvifObject = new OnvifObject();
				onvifObject.ReadXml(reader3);
				Objects.Add(onvifObject);
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString(MetadataXml.UtcTimeAttribute, UtcTime.ToUniversalTime().ToString("o", MetadataXml.Culture));
		if (Transformation != null)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.TransformationElement, MetadataXml.OnvifNamespace);
			Transformation.WriteXml(writer);
			writer.WriteEndElement();
		}
		foreach (OnvifObject @object in _objects)
		{
			writer.WriteStartElement(MetadataXml.OnvifPrefix, MetadataXml.ObjectElement, MetadataXml.OnvifNamespace);
			@object.WriteXml(writer);
			writer.WriteEndElement();
		}
	}

	public bool Equals(Frame other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (UtcTime.Equals(other.UtcTime) && _objects.SequenceEqual(other._objects))
		{
			return object.Equals(Transformation, other.Transformation);
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
		return Equals((Frame)obj);
	}

	public override int GetHashCode()
	{
		return (((UtcTime.GetHashCode() * 397) ^ ((_objects != null) ? _objects.GetHashCode() : 0)) * 397) ^ ((Transformation != null) ? Transformation.GetHashCode() : 0);
	}
}
