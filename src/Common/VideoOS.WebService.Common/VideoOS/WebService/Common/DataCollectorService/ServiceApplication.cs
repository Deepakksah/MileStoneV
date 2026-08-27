using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class ServiceApplication
{
	[DataMember]
	public string ApplicationName { get; set; }

	[DataMember]
	public string UpgradeCode { get; set; }

	[DataMember]
	public bool IsInstalled { get; set; }
}
