using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum ServiceStatus
{
	[EnumMember]
	[Obsolete("State not possible")]
	Stopped,
	[EnumMember]
	[Obsolete("Only State possible")]
	Running,
	[EnumMember]
	[Obsolete("State not possible")]
	StopPending
}
