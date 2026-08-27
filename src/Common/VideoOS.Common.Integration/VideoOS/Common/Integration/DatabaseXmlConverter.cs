using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace VideoOS.Common.Integration;

public static class DatabaseXmlConverter
{
	public static IEnumerable<StickerSize2> DeviceXmlToStickerSize(string deviceXml)
	{
		if (string.IsNullOrEmpty(deviceXml))
		{
			return Enumerable.Empty<StickerSize2>();
		}
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(deviceXml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("MarkedDataDevices");
		IEnumerable<StickerSize2> result = Enumerable.Empty<StickerSize2>();
		if (xmlNode != null)
		{
			result = xmlNode.SelectNodes("Device").Cast<XmlNode>().Select(delegate(XmlNode d)
			{
				Guid deviceId = new Guid(d.SelectSingleNode("IDDevice").InnerText);
				long size = 0L;
				if (d.SelectSingleNode("Size") != null)
				{
					size = long.Parse(d.SelectSingleNode("Size").InnerText, CultureInfo.InvariantCulture);
				}
				return new StickerSize2(deviceId, size);
			});
		}
		return result;
	}

	public static IEnumerable<Guid> XmlIdsToGuids(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		if (string.IsNullOrEmpty(xml))
		{
			throw new ArgumentException("The xml cannot be empty", "xml");
		}
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.LoadXml(xml);
		}
		catch (XmlException innerException)
		{
			throw new ArgumentException("Failed to parse xml.", "xml", innerException);
		}
		return from XmlNode d in xmlDocument.SelectNodes("/Ids/Id")
			select new Guid(d.InnerText);
	}
}
