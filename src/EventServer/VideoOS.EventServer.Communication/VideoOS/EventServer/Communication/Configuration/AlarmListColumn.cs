using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmListColumn", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
public enum AlarmListColumn
{
	[EnumMember]
	Priority,
	[EnumMember]
	PriorityName,
	[EnumMember]
	State,
	[EnumMember]
	StateName,
	[EnumMember]
	Category,
	[EnumMember]
	CategoryName,
	[EnumMember]
	Id,
	[EnumMember]
	Time,
	[EnumMember]
	Name,
	[EnumMember]
	Source,
	[EnumMember]
	Message,
	[EnumMember]
	Rule,
	[EnumMember]
	Vendor,
	[EnumMember]
	Location,
	[EnumMember]
	CustomTag,
	[EnumMember]
	Image,
	[EnumMember]
	Type,
	[EnumMember]
	ObjectValue,
	[EnumMember]
	AssignedTo,
	[EnumMember]
	ServerName
}
