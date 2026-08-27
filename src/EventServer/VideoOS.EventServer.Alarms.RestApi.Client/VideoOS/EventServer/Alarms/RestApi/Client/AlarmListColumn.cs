using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum AlarmListColumn
{
	[EnumMember(Value = "Priority")]
	Priority,
	[EnumMember(Value = "PriorityName")]
	PriorityName,
	[EnumMember(Value = "State")]
	State,
	[EnumMember(Value = "StateName")]
	StateName,
	[EnumMember(Value = "Category")]
	Category,
	[EnumMember(Value = "CategoryName")]
	CategoryName,
	[EnumMember(Value = "Id")]
	Id,
	[EnumMember(Value = "Time")]
	Time,
	[EnumMember(Value = "Name")]
	Name,
	[EnumMember(Value = "Source")]
	Source,
	[EnumMember(Value = "Message")]
	Message,
	[EnumMember(Value = "Rule")]
	Rule,
	[EnumMember(Value = "Vendor")]
	Vendor,
	[EnumMember(Value = "Location")]
	Location,
	[EnumMember(Value = "CustomTag")]
	CustomTag,
	[EnumMember(Value = "Image")]
	Image,
	[EnumMember(Value = "Type")]
	Type,
	[EnumMember(Value = "ObjectValue")]
	ObjectValue,
	[EnumMember(Value = "AssignedTo")]
	AssignedTo,
	[EnumMember(Value = "ServerName")]
	ServerName
}
