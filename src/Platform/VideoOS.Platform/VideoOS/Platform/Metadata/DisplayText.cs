using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class DisplayText : IXmlSerializable
{
	private static readonly object Lock = new object();

	private static DateTime _lastColorParseError;

	public float? CenterX { get; set; }

	public float? CenterY { get; set; }

	public bool IsBold { get; set; }

	public bool IsItalic { get; set; }

	public string FontFamily { get; set; }

	public float? Size { get; set; }

	public DisplayColor Color { get; set; }

	public string Value { get; set; }

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		if (reader == null)
		{
			throw new ArgumentNullException("reader");
		}
		reader.MoveToContent();
		int depth = reader.Depth;
		bool isEmptyElement = reader.IsEmptyElement;
		BlankAllFields();
		ReadAttributes(reader);
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
			if (reader.Depth == rootDepth + 1 && reader.NodeType == XmlNodeType.Text)
			{
				Value = reader.Value;
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	private void ReadAttributes(XmlReader reader)
	{
		while (reader.MoveToNextAttribute())
		{
			string localName = reader.LocalName;
			if ((object)MetadataXml.ColorAttribute == localName)
			{
				string argbString = reader.ReadContentAsString();
				DisplayColor color = ParseColor(argbString);
				Color = color;
			}
			bool result5;
			if ((object)MetadataXml.FontFamilyAttribute == localName)
			{
				FontFamily = reader.ReadContentAsString();
			}
			else if ((object)MetadataXml.VectorXAttribute == localName)
			{
				if (float.TryParse(reader.ReadContentAsString(), MetadataXml.FloatStyle, MetadataXml.Culture, out var result))
				{
					CenterX = result;
				}
			}
			else if ((object)MetadataXml.VectorYAttribute == localName)
			{
				if (float.TryParse(reader.ReadContentAsString(), MetadataXml.FloatStyle, MetadataXml.Culture, out var result2))
				{
					CenterY = result2;
				}
			}
			else if ((object)MetadataXml.SizeAttribute == localName)
			{
				if (float.TryParse(reader.ReadContentAsString(), MetadataXml.FloatStyle, MetadataXml.Culture, out var result3))
				{
					Size = result3;
				}
			}
			else if ((object)MetadataXml.BoldAttribute == localName)
			{
				if (bool.TryParse(reader.ReadContentAsString(), out var result4))
				{
					IsBold = result4;
				}
			}
			else if ((object)MetadataXml.ItalicAttribute == localName && bool.TryParse(reader.ReadContentAsString(), out result5))
			{
				IsItalic = result5;
			}
		}
	}

	private DisplayColor ParseColor(string argbString)
	{
		if (!DisplayColor.TryParseArgbString(argbString, out var color))
		{
			lock (Lock)
			{
				if (DateTime.UtcNow - _lastColorParseError > MetadataXml.LogIgnoreTimeSpand)
				{
					string text = string.Format(CultureInfo.InvariantCulture, "Color with value '{0}' could not be parsed", argbString);
					EnvironmentManager.Instance.Log(GetType().FullName, error: false, "ReadXml", text, null);
					_lastColorParseError = DateTime.UtcNow;
				}
			}
		}
		return color;
	}

	private void BlankAllFields()
	{
		CenterX = null;
		CenterY = null;
		IsBold = false;
		IsItalic = false;
		FontFamily = null;
		Size = null;
		Color = null;
		Value = null;
	}

	public void WriteXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		if (CenterX.HasValue)
		{
			writer.WriteAttributeString(MetadataXml.VectorXAttribute, CenterX.Value.ToString(MetadataXml.Culture));
		}
		if (CenterY.HasValue)
		{
			writer.WriteAttributeString(MetadataXml.VectorYAttribute, CenterY.Value.ToString(MetadataXml.Culture));
		}
		if (Size.HasValue)
		{
			writer.WriteAttributeString(MetadataXml.SizeAttribute, Size.Value.ToString(MetadataXml.Culture));
		}
		if (IsBold)
		{
			writer.WriteAttributeString(MetadataXml.BoldAttribute, IsBold.ToString(MetadataXml.Culture).ToLowerInvariant());
		}
		if (IsItalic)
		{
			writer.WriteAttributeString(MetadataXml.ItalicAttribute, IsItalic.ToString(MetadataXml.Culture).ToLowerInvariant());
		}
		if (FontFamily != null)
		{
			writer.WriteAttributeString(MetadataXml.FontFamilyAttribute, FontFamily);
		}
		if (Color != null)
		{
			writer.WriteAttributeString(MetadataXml.ColorAttribute, Color.ArgbString);
		}
		if (Value != null)
		{
			writer.WriteValue(Value);
		}
	}

	public void Apply(Transformation transformation)
	{
		if (transformation != null)
		{
			if (transformation.Scale != null)
			{
				CenterX *= transformation.Scale.X;
				CenterY *= transformation.Scale.Y;
			}
			if (transformation.Translate != null)
			{
				CenterX += transformation.Translate.X;
				CenterY += transformation.Translate.Y;
			}
		}
	}
}
