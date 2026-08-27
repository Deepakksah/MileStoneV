using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Alarms.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum AlarmTargetOperatorCriteria
{
	[EnumMember(Value = "id")]
	Id,
	[EnumMember(Value = "localId")]
	LocalId,
	[EnumMember(Value = "time")]
	Timestamp,
	[EnumMember(Value = "source.name")]
	SourceName,
	[EnumMember(Value = "message")]
	Message,
	[EnumMember(Value = "type")]
	Type,
	[EnumMember(Value = "analyticData.vendor")]
	AnalyticDataVendorName,
	[EnumMember(Value = "analyticData.location")]
	AnalyticDataLocation,
	[EnumMember(Value = "analyticData.object.value")]
	AnalyticDataObject,
	[EnumMember(Value = "analyticData.rule.type")]
	RuleType,
	[EnumMember(Value = "description")]
	Description,
	[EnumMember(Value = "assignedTo")]
	AssignedTo,
	[EnumMember(Value = "category.level")]
	CategoryLevel,
	[EnumMember(Value = "category.name")]
	CategoryName,
	[EnumMember(Value = "modified")]
	Modified,
	[EnumMember(Value = "name")]
	Name,
	[EnumMember(Value = "priority.level")]
	PriorityLevel,
	[EnumMember(Value = "priority.name")]
	PriorityName,
	[EnumMember(Value = "source.id")]
	SourceId,
	[EnumMember(Value = "state.level")]
	StateLevel,
	[EnumMember(Value = "state.name")]
	StateName
}
