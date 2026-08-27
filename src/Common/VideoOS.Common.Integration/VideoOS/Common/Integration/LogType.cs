using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[DataContract]
public enum LogType
{
	[EnumMember]
	AuditLog,
	[EnumMember]
	SystemLog,
	[EnumMember]
	RuleLog
}
