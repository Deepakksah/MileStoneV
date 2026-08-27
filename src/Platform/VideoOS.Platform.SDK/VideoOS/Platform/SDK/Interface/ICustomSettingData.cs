using System.Xml;

namespace VideoOS.Platform.SDK.Interface;

public interface ICustomSettingData
{
	int DataVersion { get; set; }

	XmlNode CustomSettingDataXml { get; set; }
}
