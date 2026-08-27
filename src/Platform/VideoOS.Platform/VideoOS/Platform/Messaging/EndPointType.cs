using System.Runtime.Serialization;

namespace VideoOS.Platform.Messaging;

[DataContract]
public enum EndPointType
{
	[EnumMember]
	Server = 1,
	[EnumMember]
	Client = 2,
	[EnumMember]
	Any = 99
}
