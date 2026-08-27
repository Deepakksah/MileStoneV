using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class Rectangle : IXmlSerializable, IEquatable<Rectangle>
{
	private static readonly object Lock = new object();

	private static DateTime _lastMissingAttribute;

	private static DateTime _lastColorParseError;

	public float Bottom { get; set; }

	public float Top { get; set; }

	public float Right { get; set; }

	public float Left { get; set; }

	public DisplayColor LineColor { get; set; }

	public uint? LineDisplayPixelThickness { get; set; }

	public DisplayColor FillColor { get; set; }

	public bool AllAttributesWerePresent { get; private set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		BlankAllFields();
		AllAttributesWerePresent = true;
		Top = ReadRequiredFloatAttributeValue(reader, MetadataXml.TopAttribute);
		Bottom = ReadRequiredFloatAttributeValue(reader, MetadataXml.BottomAttribute);
		Left = ReadRequiredFloatAttributeValue(reader, MetadataXml.LeftAttribute);
		Right = ReadRequiredFloatAttributeValue(reader, MetadataXml.RightAttribute);
	}

	private void BlankAllFields()
	{
		float num = (Right = 0f);
		float num3 = (Left = num);
		float top = (Bottom = num3);
		Top = top;
		FillColor = null;
		LineColor = null;
		LineDisplayPixelThickness = null;
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
					string text = string.Format(CultureInfo.InvariantCulture, "Required attribute {0} is missing", attributeName);
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", text, null);
					_lastMissingAttribute = DateTime.UtcNow;
				}
			}
		}
		return result;
	}

	public void ReadAppearanceExtensionXml(XmlReader reader)
	{
		int depth = reader.Depth;
		do
		{
			if ((object)reader.NamespaceURI != string.Empty || reader.Depth != depth + 1 || reader.NodeType != XmlNodeType.Element)
			{
				continue;
			}
			string localName = reader.LocalName;
			if ((object)MetadataXml.FillElement == localName)
			{
				string attribute = reader.GetAttribute(MetadataXml.ColorAttribute);
				if (attribute != null)
				{
					DisplayColor fillColor = ReadColor(attribute);
					FillColor = fillColor;
				}
			}
			else if ((object)MetadataXml.LineElement == localName)
			{
				string attribute2 = reader.GetAttribute(MetadataXml.ColorAttribute);
				if (attribute2 != null)
				{
					DisplayColor lineColor = ReadColor(attribute2);
					LineColor = lineColor;
				}
				if (uint.TryParse(reader.GetAttribute(MetadataXml.DisplayedThicknessInPixelsAttribute), MetadataXml.UintStyle, MetadataXml.Culture, out var result))
				{
					LineDisplayPixelThickness = result;
				}
			}
		}
		while (reader.Read());
	}

	private DisplayColor ReadColor(string colorValue)
	{
		if (!DisplayColor.TryParseArgbString(colorValue, out var color))
		{
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastColorParseError > MetadataXml.LogIgnoreTimeSpand)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "Color with value '{0}' could not be parsed", colorValue);
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", text, null);
					_lastColorParseError = DateTime.UtcNow;
				}
			}
			return null;
		}
		return color;
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString(MetadataXml.BottomAttribute, Bottom.ToString(MetadataXml.Culture));
		writer.WriteAttributeString(MetadataXml.TopAttribute, Top.ToString(MetadataXml.Culture));
		writer.WriteAttributeString(MetadataXml.RightAttribute, Right.ToString(MetadataXml.Culture));
		writer.WriteAttributeString(MetadataXml.LeftAttribute, Left.ToString(MetadataXml.Culture));
	}

	public void WriteAppearanceExtenstionXml(XmlWriter writer)
	{
		if (FillColor != null)
		{
			writer.WriteStartElement(MetadataXml.FillElement);
			writer.WriteAttributeString(MetadataXml.ColorAttribute, FillColor.ArgbString);
			writer.WriteEndElement();
		}
		if (LineColor != null || LineDisplayPixelThickness.HasValue)
		{
			writer.WriteStartElement(MetadataXml.LineElement);
			if (LineColor != null)
			{
				writer.WriteAttributeString(MetadataXml.ColorAttribute, LineColor.ArgbString);
			}
			if (LineDisplayPixelThickness.HasValue)
			{
				writer.WriteAttributeString(MetadataXml.DisplayedThicknessInPixelsAttribute, LineDisplayPixelThickness.Value.ToString(MetadataXml.Culture));
			}
			writer.WriteEndElement();
		}
	}

	public bool HasExtensionDataPresent()
	{
		if (LineColor == null && FillColor == null)
		{
			return LineDisplayPixelThickness.HasValue;
		}
		return true;
	}

	public void Apply(Transformation transformation)
	{
		if (transformation != null)
		{
			if (transformation.Scale != null)
			{
				Left *= transformation.Scale.X;
				Right *= transformation.Scale.X;
				Top *= transformation.Scale.Y;
				Bottom *= transformation.Scale.Y;
			}
			if (transformation.Translate != null)
			{
				Left += transformation.Translate.X;
				Right += transformation.Translate.X;
				Top += transformation.Translate.Y;
				Bottom += transformation.Translate.Y;
			}
		}
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "Rectangle - Left: {0}, Top: {1}, Right: {2}, Bottom: {3}", Left, Top, Right, Bottom);
	}

	public bool Equals(Rectangle other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (Bottom.Equals(other.Bottom) && Top.Equals(other.Top) && Right.Equals(other.Right))
		{
			return Left.Equals(other.Left);
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
		return Equals((Rectangle)obj);
	}

	public override int GetHashCode()
	{
		return (((((Bottom.GetHashCode() * 397) ^ Top.GetHashCode()) * 397) ^ Right.GetHashCode()) * 397) ^ Left.GetHashCode();
	}
}
