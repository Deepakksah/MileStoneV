using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config.Svc;

public class CustomSettingData
{
	public int DataVersion { get; set; }

	public XmlNode CustomSettingDataXml { get; set; }

	public CustomSettingData(ICustomSettingData customSettingData)
	{
		DataVersion = customSettingData.DataVersion;
		CustomSettingDataXml = customSettingData.CustomSettingDataXml;
	}

	public CustomSettingData()
	{
	}
}
