using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public enum RecorderConnectionStatus
{
	[EnumMember]
	NotConnected,
	[EnumMember]
	Connected,
	[EnumMember]
	ConnectionBroken
}
