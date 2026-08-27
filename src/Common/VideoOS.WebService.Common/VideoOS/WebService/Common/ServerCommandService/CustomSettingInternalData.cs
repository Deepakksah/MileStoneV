using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class CustomSettingInternalData
{
	[DataMember]
	public int DataVersion { get; set; }

	[DataMember]
	public string CustomSettingDataXml { get; set; }
}
