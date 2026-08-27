using System;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class AviData
{
	public DateTime StartTimeUtc { get; set; }

	public DateTime EndTimeUtc { get; set; }

	public double Framerate { get; set; }

	public string OutputFile { get; set; }

	public CompressorData Compressor { get; set; }

	public AviData()
	{
		Compressor = new CompressorData();
	}

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("Avi");
		writer.WriteElementString("StartTimeUtc", XmlConvert.ToString(StartTimeUtc, XmlDateTimeSerializationMode.Utc));
		writer.WriteElementString("EndTimeUtc", XmlConvert.ToString(EndTimeUtc, XmlDateTimeSerializationMode.Utc));
		writer.WriteElementString("Framerate", XmlConvert.ToString(Framerate));
		writer.WriteElementString("OutputFile", OutputFile);
		Compressor.ToXml(writer);
		writer.WriteEndElement();
	}

	public static AviData FromXml(XmlReader reader)
	{
		AviData aviData = new AviData();
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("StartTimeUtc", StringComparison.OrdinalIgnoreCase))
					{
						aviData.StartTimeUtc = XmlConvert.ToDateTime(Helper.StringFromXml(reader), XmlDateTimeSerializationMode.Utc);
						continue;
					}
					if (name.Equals("EndTimeUtc", StringComparison.OrdinalIgnoreCase))
					{
						aviData.EndTimeUtc = XmlConvert.ToDateTime(Helper.StringFromXml(reader), XmlDateTimeSerializationMode.Utc);
						continue;
					}
					if (name.Equals("Framerate", StringComparison.OrdinalIgnoreCase))
					{
						aviData.Framerate = XmlConvert.ToDouble(Helper.StringFromXml(reader));
						continue;
					}
					if (name.Equals("OutputFile", StringComparison.OrdinalIgnoreCase))
					{
						aviData.OutputFile = Helper.StringFromXml(reader);
						continue;
					}
					if (name.Equals("Compressor", StringComparison.OrdinalIgnoreCase))
					{
						aviData.Compressor = CompressorData.FromXml(reader);
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return aviData;
	}
}
