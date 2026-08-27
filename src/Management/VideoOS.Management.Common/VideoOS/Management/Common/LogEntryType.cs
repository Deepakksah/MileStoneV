using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public enum LogEntryType
{
	[EnumMember]
	Information = 1,
	[EnumMember]
	Warning = 2,
	[EnumMember]
	Error = 4
}
