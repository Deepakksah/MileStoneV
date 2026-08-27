using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.UI.Common.WPF.VideoText;

public class SlideData : IXmlSerializable
{
	private const string TextXmlName = "Text";

	private const string DisplayNameXmlName = "DisplayName";

	private const string FontFamilyNameXmlName = "FontFamilyName";

	private const string FontSizeXmlName = "FontSize";

	private const string BackgroundColorXmlName = "BackgroundColor";

	private const string ForegroundColorXmlName = "ForegroundColor";

	private const string DurationXmlName = "Duration";

	public string Text { get; set; }

	public string DisplayName { get; set; }

	public string FontFamilyName { get; set; }

	public double FontSize { get; set; }

	public string BackgroundColor { get; set; }

	public string ForegroundColor { get; set; }

	public int Duration { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		string localName = reader.LocalName;
		reader.MoveToFirstAttribute();
		do
		{
			if (reader.LocalName == "FontFamilyName")
			{
				FontFamilyName = reader.Value;
			}
			else if (reader.LocalName == "FontSize")
			{
				FontSize = double.Parse(reader.Value, CultureInfo.InvariantCulture);
			}
			else if (reader.LocalName == "BackgroundColor")
			{
				BackgroundColor = reader.Value;
			}
			else if (reader.LocalName == "ForegroundColor")
			{
				ForegroundColor = reader.Value;
			}
			else if (reader.LocalName == "Duration")
			{
				Duration = int.Parse(reader.Value, CultureInfo.InvariantCulture);
			}
		}
		while (reader.MoveToNextAttribute());
		while (reader.NodeType != XmlNodeType.Element || !(reader.LocalName == localName) || !reader.IsEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				if (reader.LocalName == "DisplayName")
				{
					DisplayName = reader.ReadString();
				}
				else if (reader.LocalName == "Text")
				{
					Text = reader.ReadString();
				}
			}
			if ((reader.NodeType == XmlNodeType.EndElement && reader.LocalName == localName) || !reader.Read())
			{
				break;
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString("FontFamilyName", FontFamilyName);
		writer.WriteAttributeString("FontSize", FontSize.ToString(CultureInfo.InvariantCulture));
		writer.WriteAttributeString("BackgroundColor", BackgroundColor);
		writer.WriteAttributeString("ForegroundColor", ForegroundColor);
		writer.WriteAttributeString("Duration", Duration.ToString(CultureInfo.InvariantCulture));
		writer.WriteStartElement("DisplayName");
		writer.WriteCData(DisplayName);
		writer.WriteEndElement();
		writer.WriteStartElement("Text");
		writer.WriteCData(Text);
		writer.WriteEndElement();
	}
}
