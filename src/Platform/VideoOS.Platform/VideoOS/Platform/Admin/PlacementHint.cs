using System.Runtime.Serialization;

namespace VideoOS.Platform.Admin;

[DataContract]
public enum PlacementHint
{
	[EnumMember(Value = "0")]
	Default,
	[EnumMember(Value = "1")]
	Basics,
	[EnumMember(Value = "2")]
	Servers,
	[EnumMember(Value = "3")]
	Devices,
	[EnumMember(Value = "4")]
	Client,
	[EnumMember(Value = "5")]
	RulesEvents,
	[EnumMember(Value = "6")]
	Security,
	[EnumMember(Value = "7")]
	Dashboard,
	[EnumMember(Value = "8")]
	Logs,
	[EnumMember(Value = "9")]
	Hidden,
	[EnumMember(Value = "10")]
	Root,
	[EnumMember(Value = "11")]
	ParentItemTree,
	[EnumMember(Value = "12")]
	HideInLeftTree
}
