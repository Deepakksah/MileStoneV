using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace VideoOS.Management.Common;

public class RecordingServerDiskUsages : List<RecordingServerDiskUsage>
{
	public static RecordingServerDiskUsages GetFromXml(string diskUsageXml)
	{
		RecordingServerDiskUsages recordingServerDiskUsages = new RecordingServerDiskUsages();
		using StringReader input = new StringReader(diskUsageXml);
		XmlReader xmlReader = new XmlTextReader(input);
		if (xmlReader.ReadToFollowing("DiskUsage"))
		{
			if (!DateTimeOffset.TryParseExact(xmlReader.GetAttribute("LocalTime"), "o", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
			{
				throw new ArgumentException("Invalid disk usage xml - LocalTime attribute can not be parsed");
			}
			if (xmlReader.ReadToDescendant("Disk"))
			{
				do
				{
					RecordingServerDiskUsage recordingServerDiskUsage = new RecordingServerDiskUsage();
					recordingServerDiskUsage.LastUpdated = result;
					recordingServerDiskUsage.Path = xmlReader.GetAttribute("Path");
					if (!string.IsNullOrEmpty(recordingServerDiskUsage.Path))
					{
						if (!long.TryParse(xmlReader.GetAttribute("Size"), out var result2))
						{
							throw new ArgumentException("Invalid disk usage xml - Size attribute can not be parsed");
						}
						recordingServerDiskUsage.TotalSize = result2;
						if (!long.TryParse(xmlReader.GetAttribute("MediaData"), out result2))
						{
							throw new ArgumentException("Invalid disk usage xml - MediaData attribute can not be parsed");
						}
						recordingServerDiskUsage.MediaData = result2;
						if (!long.TryParse(xmlReader.GetAttribute("OtherData"), out result2))
						{
							throw new ArgumentException("Invalid disk usage xml - OtherData attribute can not be parsed");
						}
						recordingServerDiskUsage.OtherData = result2;
						if (!long.TryParse(xmlReader.GetAttribute("Free"), out result2))
						{
							throw new ArgumentException("Invalid disk usage xml - Free attribute can not be parsed");
						}
						recordingServerDiskUsage.Free = result2;
						recordingServerDiskUsages.Add(recordingServerDiskUsage);
					}
				}
				while (xmlReader.ReadToNextSibling("Disk"));
				return recordingServerDiskUsages;
			}
			throw new ArgumentException("Invalid disk usage xml - Disk element not found");
		}
		throw new ArgumentException("Invalid disk usage xml - DiskUsage element not found");
	}
}
