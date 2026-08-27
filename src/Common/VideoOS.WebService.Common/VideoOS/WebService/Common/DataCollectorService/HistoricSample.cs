using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class HistoricSample
{
	[DataMember]
	public DateTime Time { get; set; }

	[DataMember]
	public float Value { get; set; }
}
