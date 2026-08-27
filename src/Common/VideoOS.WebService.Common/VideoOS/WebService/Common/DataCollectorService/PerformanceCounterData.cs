using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class PerformanceCounterData
{
	[DataMember]
	public int DataCollectorId { get; set; }

	[DataMember]
	public PerformanceCounterValue[] Values { get; set; }
}
