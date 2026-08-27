using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class CompressorData
{
	public int Handler { get; set; }

	public int Flags { get; set; }

	public int Quality { get; set; }

	public int DataRate { get; set; }

	public int KeyFrames { get; set; }

	public byte[] ConfigData { get; set; }

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("Compressor");
		writer.WriteElementString("Handler", XmlConvert.ToString(Handler));
		writer.WriteElementString("Flags", XmlConvert.ToString(Flags));
		writer.WriteElementString("Quality", XmlConvert.ToString(Quality));
		writer.WriteElementString("DataRate", XmlConvert.ToString(DataRate));
		writer.WriteElementString("KeyFrames", XmlConvert.ToString(KeyFrames));
		writer.WriteElementString("ConfigDataBase64", (ConfigData == null) ? string.Empty : Convert.ToBase64String(ConfigData));
		writer.WriteEndElement();
	}

	public static CompressorData FromXml(XmlReader reader)
	{
		CompressorData compressorData = new CompressorData();
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("Handler", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.Handler = Convert.ToInt32(Helper.StringFromXml(reader), NumberFormatInfo.InvariantInfo);
						continue;
					}
					if (name.Equals("Flags", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.Flags = Convert.ToInt32(Helper.StringFromXml(reader), NumberFormatInfo.InvariantInfo);
						continue;
					}
					if (name.Equals("Quality", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.Quality = Convert.ToInt32(Helper.StringFromXml(reader), NumberFormatInfo.InvariantInfo);
						continue;
					}
					if (name.Equals("DataRate", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.DataRate = Convert.ToInt32(Helper.StringFromXml(reader), NumberFormatInfo.InvariantInfo);
						continue;
					}
					if (name.Equals("KeyFrames", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.KeyFrames = Convert.ToInt32(Helper.StringFromXml(reader), NumberFormatInfo.InvariantInfo);
						continue;
					}
					if (name.Equals("ConfigDataBase64", StringComparison.OrdinalIgnoreCase))
					{
						compressorData.ConfigData = Convert.FromBase64String(Helper.StringFromXml(reader));
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return compressorData;
	}
}
