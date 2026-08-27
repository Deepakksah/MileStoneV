using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum EventGeneratorSubtype
{
	[EnumMember]
	Unknown,
	[EnumMember]
	UserDefined,
	[EnumMember]
	System,
	[EnumMember]
	Generic
}
