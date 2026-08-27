using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ExceptionStatusInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public enum ExceptionStatus
{
	[EnumMember]
	Inactive,
	[EnumMember]
	InProgress,
	[EnumMember]
	Succeeded,
	[EnumMember]
	Failed,
	[EnumMember]
	Stopping,
	[EnumMember]
	Stopped
}
