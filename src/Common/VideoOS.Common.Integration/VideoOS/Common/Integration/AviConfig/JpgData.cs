using System;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class JpgData
{
	public DateTime StartTimeUtc { get; set; }

	public int NoOfImages { get; set; }

	public int TimeBtwMs { get; set; }

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("Jpg");
		writer.WriteElementString("StartTimeUtc", XmlConvert.ToString(StartTimeUtc, XmlDateTimeSerializationMode.Utc));
		writer.WriteElementString("NoOfImages", XmlConvert.ToString(NoOfImages));
		writer.WriteElementString("TimeBtwMs", XmlConvert.ToString(TimeBtwMs));
		writer.WriteEndElement();
	}

	public static JpgData FromXml(XmlReader reader)
	{
		JpgData jpgData = new JpgData();
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
						jpgData.StartTimeUtc = XmlConvert.ToDateTime(Helper.StringFromXml(reader), XmlDateTimeSerializationMode.Utc);
						continue;
					}
					if (name.Equals("NoOfImages", StringComparison.OrdinalIgnoreCase))
					{
						jpgData.NoOfImages = XmlConvert.ToInt32(Helper.StringFromXml(reader));
						continue;
					}
					if (name.Equals("TimeBtwMs", StringComparison.OrdinalIgnoreCase))
					{
						jpgData.TimeBtwMs = XmlConvert.ToInt32(Helper.StringFromXml(reader));
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return jpgData;
	}
}
