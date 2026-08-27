using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
[Flags]
public enum CommunicationErrors
{
	[EnumMember]
	None = 0,
	[EnumMember]
	SslCommunication = 1,
	[EnumMember]
	AuthorizationError = 2
}
