using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.MIP.Communication.Client.EventConverter;

public static class MetaDataParser
{
	public static ParseMetadataResult ParseMetadata(IReadOnlyDictionary<string, EventMetaData> metadata)
	{
		List<Guid> list = new List<Guid>();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		if (metadata != null && metadata.Any())
		{
			foreach (KeyValuePair<string, EventMetaData> item in metadata)
			{
				if (!item.Equals(default(KeyValuePair<string, EventMetaData>)))
				{
					if (item.Key.ToUpperInvariant() == "TARGETDEVICES")
					{
						list.AddRange(ParseTargetDevicesMetadata(item.Value.GetValueAsString()));
					}
					else if (!dictionary.ContainsKey(item.Key))
					{
						dictionary.Add(item.Key, item.Value.GetValueAsString());
					}
				}
			}
		}
		return new ParseMetadataResult(list.Distinct(), dictionary);
	}

	private static List<Guid> ExtractDeviceIdsFromDeviceGroupId(Guid deviceGrupId)
	{
		Data data = Server.Instance.Data;
		ConfigurationDataSet.DeviceGroupMembersDataTable deviceGroupMembers = data.LoadDeviceGroupMembersByDeviceGroupId(deviceGrupId).DeviceGroupMembers;
		List<Guid> list = deviceGroupMembers.Select((ConfigurationDataSet.DeviceGroupMembersRow gm) => Guid.Parse(gm.IDDevice)).ToList();
		ConfigurationDataSet.DeviceGroupsDataTable deviceGroups = data.LoadDeviceGroupsByParentDeviceGroupId(deviceGrupId).DeviceGroups;
		foreach (ConfigurationDataSet.DeviceGroupsRow item in deviceGroups)
		{
			List<Guid> collection = ExtractDeviceIdsFromDeviceGroupId(Guid.Parse(item.IDDeviceGroup));
			list.AddRange(collection);
		}
		return list;
	}

	private static IEnumerable<Guid> ParseTargetDevicesMetadata(string value)
	{
		List<Guid> list = new List<Guid>();
		XmlDocument xmlDocument = new XmlDocument();
		XmlReaderSettings settings = new XmlReaderSettings
		{
			CheckCharacters = true,
			DtdProcessing = DtdProcessing.Prohibit,
			MaxCharactersFromEntities = 1024L
		};
		byte[] bytes = Encoding.Unicode.GetBytes(value.ToUpperInvariant());
		using (MemoryStream input = new MemoryStream(bytes))
		{
			XmlReader reader = XmlReader.Create(input, settings);
			xmlDocument.Load(reader);
		}
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/TARGETDEVICES/DEVICEIDS/DEVICEID");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item in xmlNodeList)
			{
				if (!string.IsNullOrEmpty(item?.InnerText))
				{
					Guid guid = Guid.Parse(item.InnerText);
					if (guid != Guid.Empty)
					{
						list.Add(guid);
					}
				}
			}
		}
		XmlNodeList xmlNodeList2 = xmlDocument.SelectNodes("/TARGETDEVICES/DEVICEGROUPIDS/DEVICEGROUPID");
		if (xmlNodeList != null)
		{
			foreach (XmlNode item2 in xmlNodeList2)
			{
				if (!string.IsNullOrEmpty(item2?.InnerText))
				{
					Guid guid2 = Guid.Parse(item2.InnerText);
					if (guid2 != Guid.Empty)
					{
						List<Guid> collection = ExtractDeviceIdsFromDeviceGroupId(guid2);
						list.AddRange(collection);
					}
				}
			}
		}
		return list.Distinct();
	}
}
