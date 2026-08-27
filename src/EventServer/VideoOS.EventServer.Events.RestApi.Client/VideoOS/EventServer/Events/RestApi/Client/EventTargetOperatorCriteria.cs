using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum EventTargetOperatorCriteria
{
	[EnumMember(Value = "id")]
	Id,
	[EnumMember(Value = "localId")]
	LocalId,
	[EnumMember(Value = "time")]
	Timestamp,
	[EnumMember(Value = "source.id")]
	SourceId,
	[EnumMember(Value = "source.name")]
	SourceName,
	[EnumMember(Value = "message")]
	Message,
	[EnumMember(Value = "type")]
	Type,
	[EnumMember(Value = "type.name")]
	TypeName,
	[EnumMember(Value = "typeGroup")]
	TypeGroup,
	[EnumMember(Value = "analyticData.vendor")]
	AnalyticDataVendorName,
	[EnumMember(Value = "analyticData.object.value")]
	AnalyticDataObjectValue,
	[EnumMember(Value = "analyticData.object.data")]
	AnalyticDataObjectData,
	[EnumMember(Value = "analyticData.rule.name")]
	AnalyticDataRulesName,
	[EnumMember(Value = "name")]
	Name,
	[EnumMember(Value = "tag")]
	Tag
}
