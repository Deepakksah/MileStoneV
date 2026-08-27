using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class PerformanceCounterValue
{
	[DataMember]
	public string CategoryName { get; set; }

	[DataMember]
	public string CounterName { get; set; }

	[DataMember]
	public string InstanceName { get; set; }

	[DataMember]
	public DateTime CollectionTime { get; set; }

	[DataMember]
	public long RawValue { get; set; }

	[DataMember]
	public float CalculatedValue { get; set; }

	public bool SendImmediately { get; set; }
}
