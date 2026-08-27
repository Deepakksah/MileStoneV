using System;
using System.Xml;

namespace VideoOS.Common.Integration.AviConfig;

public sealed class MediaSource
{
	public Guid DeviceId { get; set; }

	public string MediaType { get; set; }

	public string Address { get; set; }

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("MediaSource");
		writer.WriteElementString("DeviceId", XmlConvert.ToString(DeviceId));
		writer.WriteElementString("MediaType", MediaType);
		writer.WriteElementString("Address", Address);
		writer.WriteEndElement();
	}

	public static MediaSource FromXml(XmlReader reader)
	{
		MediaSource mediaSource = new MediaSource();
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("DeviceId", StringComparison.OrdinalIgnoreCase))
					{
						mediaSource.DeviceId = new Guid(Helper.StringFromXml(reader));
						continue;
					}
					if (name.Equals("MediaType", StringComparison.OrdinalIgnoreCase))
					{
						mediaSource.MediaType = Helper.StringFromXml(reader);
						continue;
					}
					if (name.Equals("Address", StringComparison.OrdinalIgnoreCase))
					{
						mediaSource.Address = Helper.StringFromXml(reader);
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return mediaSource;
	}
}
