using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class CounterStateChangeData
{
	[DataMember]
	public int DataCollectorId { get; set; }

	[DataMember]
	public string CounterRuleId { get; set; }

	[DataMember]
	public string CounterPath { get; set; }

	[DataMember]
	public int State { get; set; }

	[DataMember]
	public double TriggerValue { get; set; }
}
