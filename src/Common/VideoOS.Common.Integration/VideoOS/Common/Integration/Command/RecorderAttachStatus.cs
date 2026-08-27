using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum RecorderAttachStatus
{
	[EnumMember]
	NotAttached,
	[EnumMember]
	AttachedPending,
	[EnumMember]
	Attached
}
