using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CustomSetting
{
	[DataMember]
	public CustomSettingInfo CustomSettingInfo { get; set; }

	[DataMember]
	public CustomSettingData CustomSettingData { get; set; }
}
