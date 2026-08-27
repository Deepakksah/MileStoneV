using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class AviConfigData
{
	public MediaSources MediaSources { get; set; }

	public JpgData Jpg { get; set; }

	public AviData Avi { get; set; }

	public string TempDirectory { get; set; }

	public bool GetCodec { get; set; }

	public AviConfigData()
	{
		MediaSources = new MediaSources();
		Jpg = new JpgData();
		Avi = new AviData();
		GetCodec = false;
	}

	public static AviConfigData FromXml(string configuration)
	{
		AviConfigData aviConfigData = new AviConfigData();
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		using (XmlReader xmlReader = XmlReader.Create(new StringReader(configuration), xmlReaderSettings))
		{
			bool isEmptyElement = xmlReader.IsEmptyElement;
			xmlReader.Read();
			if (xmlReader.NodeType == XmlNodeType.XmlDeclaration)
			{
				xmlReader.Read();
			}
			if (!isEmptyElement)
			{
				while (xmlReader.NodeType == XmlNodeType.Element)
				{
					string name = xmlReader.Name;
					if (name != null)
					{
						if (name.Equals("TempDir", StringComparison.OrdinalIgnoreCase))
						{
							aviConfigData.TempDirectory = Helper.StringFromXml(xmlReader);
							continue;
						}
						if (name.Equals("GetCodec", StringComparison.OrdinalIgnoreCase))
						{
							aviConfigData.GetCodec = bool.Parse(Helper.StringFromXml(xmlReader));
							continue;
						}
						if (name.Equals("MediaSources", StringComparison.OrdinalIgnoreCase))
						{
							aviConfigData.MediaSources = MediaSources.FromXml(xmlReader);
							continue;
						}
						if (name.Equals("Jpg", StringComparison.OrdinalIgnoreCase))
						{
							aviConfigData.Jpg = JpgData.FromXml(xmlReader);
							continue;
						}
						if (name.Equals("Avi", StringComparison.OrdinalIgnoreCase))
						{
							aviConfigData.Avi = AviData.FromXml(xmlReader);
							continue;
						}
					}
					xmlReader.Read();
				}
				xmlReader.ReadEndElement();
			}
		}
		return aviConfigData;
	}

	public void ToXml(XmlWriter writer)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.WriteStartElement("AviGen");
		writer.WriteElementString("TempDir", TempDirectory);
		writer.WriteElementString("GetCodec", GetCodec.ToString(CultureInfo.InvariantCulture));
		MediaSources.ToXml(writer);
		Jpg.ToXml(writer);
		Avi.ToXml(writer);
		writer.WriteEndElement();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
		{
			OmitXmlDeclaration = true,
			Encoding = Encoding.Unicode
		});
		ToXml(xmlWriter);
		xmlWriter.Close();
		return stringBuilder.ToString();
	}
}
