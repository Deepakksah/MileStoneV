using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[DataContract]
public enum DatabaseResponseStatus
{
	[EnumMember]
	Success = 1,
	[EnumMember]
	Error,
	[EnumMember]
	Warning
}
