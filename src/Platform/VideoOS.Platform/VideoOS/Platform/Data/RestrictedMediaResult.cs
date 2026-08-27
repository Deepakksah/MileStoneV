using System;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[Serializable]
[DataContract(Name = "RestrictedMediaResult", Namespace = "urn:milestone-systems")]
public class RestrictedMediaResult
{
	[DataMember]
	public RestrictedMedia RestrictedMedia { get; set; }

	[DataMember]
	public FaultDevice[] FaultDevices { get; set; }

	[DataMember]
	public RestrictedMediaResultStatus Status { get; set; }

	[DataMember]
	public WarningDevice[] WarningDevices { get; set; }
}
