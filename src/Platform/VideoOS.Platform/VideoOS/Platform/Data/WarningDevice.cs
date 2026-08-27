using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[Serializable]
[DataContract(Name = "WarningDevice", Namespace = "urn:milestone-systems")]
public class WarningDevice
{
	[DataMember]
	public FQID FQID { get; set; }

	[DataMember]
	public int ErrorCode { get; set; }

	[DataMember]
	public string Message { get; set; }
}
