using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "MarkedDataReferenceInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class MarkedDataReference2
{
	[DataMember]
	public string Reference { get; set; }

	[DataMember]
	public DateTime Time { get; set; }

	[DataMember]
	public string[] Devices { get; set; }
}
