using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum EvidenceLockFailedRemoveEntityType
{
	[EnumMember]
	Hardware,
	[EnumMember]
	RecordingServer
}
