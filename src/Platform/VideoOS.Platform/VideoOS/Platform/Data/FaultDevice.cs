using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[Serializable]
[DataContract(Name = "FaultDevice", Namespace = "urn:milestone-systems")]
public class FaultDevice
{
	[DataMember]
	public FQID FQID { get; set; }

	[DataMember]
	public int ErrorCode { get; set; }

	[DataMember]
	public string Message { get; set; }
}
