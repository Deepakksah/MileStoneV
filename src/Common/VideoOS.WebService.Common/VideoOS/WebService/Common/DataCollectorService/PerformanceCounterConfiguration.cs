using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class PerformanceCounterConfiguration
{
	[DataMember]
	public PerformanceCounterInstance CounterInstance { get; set; }

	[DataMember]
	public int SampleFrequency { get; set; }
}
