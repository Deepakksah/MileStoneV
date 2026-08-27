using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
public enum ComparisonOperatorCriteria
{
	[EnumMember(Value = "equals")]
	Equals,
	[EnumMember(Value = "notEquals")]
	NotEquals,
	[EnumMember(Value = "lt")]
	LowerThan,
	[EnumMember(Value = "gt")]
	GreaterThan,
	[EnumMember(Value = "contains")]
	Contains,
	[EnumMember(Value = "startsWith")]
	StartsWith,
	[EnumMember(Value = "oneOf")]
	OneOf,
	[EnumMember(Value = "otherThan")]
	OtherThan,
	[EnumMember(Value = "containsOneOf")]
	ContainsOneOf
}
