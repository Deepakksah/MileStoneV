using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class CounterStateRuleConfiguration
{
	[DataMember]
	public string CounterPath { get; set; }

	[DataMember]
	public float WarningThreshold { get; set; }

	[DataMember]
	public float CriticalThreshold { get; set; }

	[DataMember]
	public int TimeWindow { get; set; }

	[DataMember]
	public string CounterRuleId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string CategoryName { get; set; }

	[DataMember]
	public string CounterName { get; set; }
}
