using System.Runtime.Serialization;
using System.Xml;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CustomSettingData
{
	[DataMember]
	public int DataVersion { get; set; }

	[DataMember]
	public XmlNode CustomSettingDataXml { get; set; }
}
