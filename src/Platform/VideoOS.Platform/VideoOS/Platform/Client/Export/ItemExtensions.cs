using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Client.Export;

public static class ItemExtensions
{
	public static IEnumerable<TimeInterval> GetExportIntervals(this Item item)
	{
		if (item == null || !item.Properties.ContainsKey(ClientControl.ExportTimeIntervals))
		{
			return Enumerable.Empty<TimeInterval>();
		}
		return from interval in XElement.Parse(item.Properties[ClientControl.ExportTimeIntervals]).Descendants("TimeInterval")
			select CreateTimeInterval(interval.Attribute("beginTime").Value, interval.Attribute("endTime").Value);
	}

	private static TimeInterval CreateTimeInterval(string beginTime, string endTime)
	{
		return new TimeInterval(DateTime.ParseExact(beginTime, "O", CultureInfo.InvariantCulture), DateTime.ParseExact(endTime, "O", CultureInfo.InvariantCulture));
	}

	public static Dictionary<string, string> GetViewItemProperties(this Item item)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (item == null)
		{
			return dictionary;
		}
		if (!item.Properties.ContainsKey("xml"))
		{
			return item.Properties;
		}
		foreach (XElement item2 in XElement.Parse(item.Properties["xml"]).XPathSelectElements("/iteminfo/viewitem/properties/property"))
		{
			dictionary.Add(item2.Attribute("name").Value, item2.Attribute("value").Value);
		}
		return dictionary;
	}
}
