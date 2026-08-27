using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum ServerFileSystemServerType
{
	[EnumMember]
	RecordingServer,
	[EnumMember]
	ManagementServer
}
