using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum LogLevel
{
	[EnumMember]
	Undefined,
	[EnumMember]
	Info,
	[EnumMember]
	Warning,
	[EnumMember]
	Error
}
