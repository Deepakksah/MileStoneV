using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternal
{
	[DataMember]
	public CustomSettingInfo CustomSettingInfo { get; set; }

	[DataMember]
	public CustomSettingInternalData CustomSettingData { get; set; }
}
