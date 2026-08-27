using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseWhitelist : ILicenseWhitelist
{
	private readonly List<WhitelistMatchHelper> _drivers;

	private readonly List<WhitelistMatchHelper> _devices;

	private readonly Dictionary<string, List<WhitelistMatchHelper>> _matchHelpersByDriverName;

	private readonly Dictionary<string, List<WhitelistMatchHelper>> _matchHelpersByDetectedModel;

	public string Version { get; }

	public int MaximumChannelCount { get; }

	public LicenseWhitelist(XmlNode xmlWhitelistNode)
	{
		if (xmlWhitelistNode == null)
		{
			throw new ArgumentNullException("xmlWhitelistNode");
		}
		if (!string.Equals(xmlWhitelistNode.Name, "Whitelist", StringComparison.Ordinal))
		{
			throw new ArgumentException("The root node contains an invalid name", "xmlWhitelistNode");
		}
		XmlNode xmlNode = xmlWhitelistNode.Attributes?.GetNamedItem("Version");
		if (xmlNode == null)
		{
			throw new ArgumentException("Whitelist must have Version", "xmlWhitelistNode");
		}
		Version = xmlNode.Value;
		if (int.TryParse((xmlWhitelistNode.Attributes?.GetNamedItem("MaximumChannelCount") ?? throw new ArgumentException("Whitelist must have MaximumChannelCount", "xmlWhitelistNode"))?.Value, out var result))
		{
			if (result <= 0)
			{
				throw new ArgumentException("MaximumChannelCount must be greater than zero", "xmlWhitelistNode");
			}
			MaximumChannelCount = result;
			_drivers = LoadList(xmlWhitelistNode, "Drivers", SearchType.Exact);
			_devices = LoadList(xmlWhitelistNode, "Devices", SearchType.Partial);
			_matchHelpersByDriverName = new Dictionary<string, List<WhitelistMatchHelper>>(StringComparer.Ordinal);
			_matchHelpersByDetectedModel = new Dictionary<string, List<WhitelistMatchHelper>>(StringComparer.Ordinal);
			return;
		}
		throw new ArgumentException("MaximumChannelCount must be an integer greater than zero", "xmlWhitelistNode");
	}

	public bool IsHardwareWhitelisted(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			return false;
		}
		int hardwareInfoChannels = hardwareInfo.Devices.Where((IDeviceInfo d) => d.DeviceType.Equals("Camera")).Count();
		if (!string.IsNullOrEmpty(hardwareInfo.DriverNameUpper))
		{
			if (!_matchHelpersByDriverName.TryGetValue(hardwareInfo.DriverNameUpper, out var value))
			{
				value = (from d in _drivers
					where d.IsMatch(hardwareInfo.DriverNameUpper)
					orderby d.MaximumChannelCount descending
					select d).Take(1).ToList();
				_matchHelpersByDriverName[hardwareInfo.DriverNameUpper] = value;
			}
			if (value.Any((WhitelistMatchHelper m) => hardwareInfoChannels <= m.MaximumChannelCount))
			{
				hardwareInfo.ChannelLicenseCount = 1;
				return true;
			}
		}
		if (!string.IsNullOrEmpty(hardwareInfo.DetectedModelUpper))
		{
			if (!_matchHelpersByDetectedModel.TryGetValue(hardwareInfo.DetectedModelUpper, out var value2))
			{
				value2 = (from d in _devices
					where d.IsMatch(hardwareInfo.DetectedModelUpper)
					orderby d.MaximumChannelCount descending
					select d).Take(1).ToList();
				_matchHelpersByDetectedModel[hardwareInfo.DetectedModelUpper] = value2;
			}
			if (value2.Any((WhitelistMatchHelper m) => hardwareInfoChannels <= m.MaximumChannelCount))
			{
				hardwareInfo.ChannelLicenseCount = 1;
				return true;
			}
		}
		hardwareInfo.ChannelLicenseCount = GetChannelLicenseCount(hardwareInfo);
		return false;
	}

	private static int GetChannelLicenseCount(IHardwareInfo hardwareInfo)
	{
		int num = hardwareInfo.EnabledCameras.Count();
		if (num == 0 && hardwareInfo.Enabled)
		{
			num = 1;
		}
		return num;
	}

	private List<WhitelistMatchHelper> LoadList(XmlNode xmlWhitelistNode, string nodeName, SearchType defaultSearchType)
	{
		List<WhitelistMatchHelper> list = new List<WhitelistMatchHelper>();
		XmlNode xmlNode = xmlWhitelistNode.SelectSingleNode("./" + nodeName);
		if (xmlNode != null)
		{
			foreach (XmlNode item in xmlNode)
			{
				if (item.Attributes != null && item.Attributes["Name"] != null)
				{
					string innerText = item.Attributes["Name"].InnerText;
					string text = item.Attributes["SearchType"]?.InnerText;
					if (string.IsNullOrEmpty(text))
					{
						text = defaultSearchType.ToString();
					}
					if (!int.TryParse(item.Attributes["MaximumChannelCount"]?.InnerText, out var result) || result <= 0)
					{
						result = MaximumChannelCount;
					}
					list.Add(new WhitelistMatchHelper(innerText, text, result));
				}
			}
		}
		return list;
	}
}
