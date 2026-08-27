using System;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;

public class DeviceSettingsProxy : IDeviceSettingsProxy
{
	public bool Validate(IGeminiRequestContext requestContext, string deviceId, string xmlSettings)
	{
		try
		{
			DriverSettingsBusiness.QueryDeviceSettings(requestContext, deviceId, out var mainDefinition, out var hwid, out var _);
			DeviceDefinition deviceDefinition = mainDefinition.HardwareDefinition.GetDeviceDefinition(hwid);
			DriverSettings driverSettings = new MultiDriverSettings(xmlSettings).ConvertToDriverSettings(deviceDefinition);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlSettings);
			if (xmlDocument.SelectNodes("/properties/settings/setting")?.Count != driverSettings.AllEntries.Count())
			{
				return false;
			}
			DriverSettingsBusiness.ValidateDeviceSettings(requestContext, deviceId, driverSettings);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
