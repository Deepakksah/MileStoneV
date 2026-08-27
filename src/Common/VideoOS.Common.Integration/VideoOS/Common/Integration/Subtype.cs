using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum Subtype
{
	[EnumMember]
	Unknown,
	[EnumMember]
	Server,
	[EnumMember]
	Device,
	[EnumMember]
	Disk,
	[EnumMember]
	Storage
}
