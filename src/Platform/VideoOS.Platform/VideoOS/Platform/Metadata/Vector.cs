using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Vector : IXmlSerializable, IEquatable<Vector>
{
	private static readonly object Lock = new object();

	private static DateTime _lastMissingAttribute;

	public float X { get; set; }

	public float Y { get; set; }

	public bool AllAttributesWerePresent { get; private set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		AllAttributesWerePresent = true;
		X = ReadRequiredFloatAttributeValue(reader, MetadataXml.VectorXAttribute);
		Y = ReadRequiredFloatAttributeValue(reader, MetadataXml.VectorYAttribute);
	}

	private float ReadRequiredFloatAttributeValue(XmlReader reader, string attributeName)
	{
		if (!float.TryParse(reader.GetAttribute(attributeName), MetadataXml.FloatStyle, MetadataXml.Culture, out var result))
		{
			AllAttributesWerePresent = false;
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastMissingAttribute > MetadataXml.LogIgnoreTimeSpand)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "Required attribute '{0}' could not be parsed or is missing", attributeName);
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", text, null);
					_lastMissingAttribute = DateTime.UtcNow;
				}
			}
		}
		return result;
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString(MetadataXml.VectorXAttribute, X.ToString(MetadataXml.Culture));
		writer.WriteAttributeString(MetadataXml.VectorYAttribute, Y.ToString(MetadataXml.Culture));
	}

	public bool Equals(Vector other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (X.Equals(other.X))
		{
			return Y.Equals(other.Y);
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
		return Equals((Vector)obj);
	}

	public override int GetHashCode()
	{
		return (X.GetHashCode() * 397) ^ Y.GetHashCode();
	}
}
