using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.UI.Common.WPF.VideoText;

public static class SlideSerializer
{
	public const string PreSlidesXmlName = "PreSlides";

	public const string PostSlidesXmlName = "PostSlides";

	private const string SlidesXmlName = "Slides";

	private const string SlideXmlName = "Slide";

	public static string SerializeSlides(IList<SlideData> preSlides, IList<SlideData> postSlides)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
		xmlWriter.WriteStartElement("Slides");
		WriteXml(xmlWriter, preSlides, postSlides);
		xmlWriter.WriteEndElement();
		xmlWriter.Flush();
		return stringBuilder.ToString();
	}

	public static void WriteXml(XmlWriter writer, IList<SlideData> preSlides, IList<SlideData> postSlides)
	{
		writer.WriteStartElement("PreSlides");
		foreach (SlideData preSlide in preSlides)
		{
			writer.WriteStartElement("Slide");
			preSlide.WriteXml(writer);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		writer.WriteStartElement("PostSlides");
		foreach (SlideData postSlide in postSlides)
		{
			writer.WriteStartElement("Slide");
			postSlide.WriteXml(writer);
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
	}

	public static void DeserializeSlides(string serialized, IList<SlideData> preSlides, IList<SlideData> postSlides)
	{
		if (!string.IsNullOrWhiteSpace(serialized))
		{
			ReadXml(XmlReader.Create(new StringReader(serialized)), preSlides, postSlides);
		}
	}

	public static void ReadXml(XmlReader reader, IList<SlideData> preSlides, IList<SlideData> postSlides)
	{
		string localName = reader.LocalName;
		while (reader.NodeType != XmlNodeType.Element || !(reader.LocalName == localName) || !reader.IsEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				if (reader.LocalName == "PreSlides")
				{
					ReadSlideListXml(reader, preSlides);
				}
				else if (reader.LocalName == "PostSlides")
				{
					ReadSlideListXml(reader, postSlides);
				}
			}
			if ((reader.NodeType == XmlNodeType.EndElement && reader.LocalName == localName) || !reader.Read())
			{
				break;
			}
		}
	}

	public static void ReadSlideListXml(XmlReader reader, ICollection<SlideData> slides)
	{
		string localName = reader.LocalName;
		while (reader.NodeType != XmlNodeType.Element || !(reader.LocalName == localName) || !reader.IsEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Slide")
			{
				SlideData slideData = new SlideData();
				slideData.ReadXml(reader);
				slides.Add(slideData);
			}
			if ((reader.NodeType == XmlNodeType.EndElement && reader.LocalName == localName) || !reader.Read())
			{
				break;
			}
		}
	}
}
