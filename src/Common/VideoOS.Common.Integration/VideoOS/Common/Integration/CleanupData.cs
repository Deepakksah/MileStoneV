using System;
using System.Xml;

namespace VideoOS.Common.Integration;

[Serializable]
public sealed class CleanupData
{
	public long MaxSize { get; set; }

	public int MaxMinutes { get; set; }

	public CleanupData()
	{
	}

	public void ToXml(XmlWriter writer, string element)
	{
		writer.WriteStartElement(element);
		writer.WriteElementString("max_mb", XmlConvert.ToString(MaxSize));
		writer.WriteElementString("max_minutes", XmlConvert.ToString(MaxMinutes));
		writer.WriteEndElement();
	}

	public static CleanupData FromXml(XmlReader reader)
	{
		CleanupData cleanupData = new CleanupData();
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("max_mb", StringComparison.OrdinalIgnoreCase))
					{
						cleanupData.MaxSize = XmlConvert.ToInt32(Helper.StringFromXml(reader));
						continue;
					}
					if (name.Equals("max_minutes", StringComparison.OrdinalIgnoreCase))
					{
						cleanupData.MaxMinutes = XmlConvert.ToInt32(Helper.StringFromXml(reader));
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return cleanupData;
	}
}
