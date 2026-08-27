using System.Runtime.Serialization;

namespace VideoOS.Platform.Admin;

[DataContract]
public enum Category
{
	[EnumMember(Value = "0")]
	Server,
	[EnumMember(Value = "1")]
	VideoIn,
	[EnumMember(Value = "2")]
	VideoOut,
	[EnumMember(Value = "3")]
	AudioIn,
	[EnumMember(Value = "4")]
	AudioOut,
	[EnumMember(Value = "5")]
	TriggerIn,
	[EnumMember(Value = "6")]
	TriggerOut,
	[EnumMember(Value = "7")]
	Text,
	[EnumMember(Value = "8")]
	Unknown,
	[EnumMember(Value = "9")]
	Layout
}
