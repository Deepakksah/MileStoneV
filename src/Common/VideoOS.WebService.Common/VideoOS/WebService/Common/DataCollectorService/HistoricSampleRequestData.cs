using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class HistoricSampleRequestData
{
	[DataMember]
	public int DataCollectorId { get; set; }

	[DataMember]
	public string CounterPath { get; set; }

	[DataMember]
	public DateTime FromTime { get; set; }

	[DataMember]
	public int MaxSampleCount { get; set; }
}
