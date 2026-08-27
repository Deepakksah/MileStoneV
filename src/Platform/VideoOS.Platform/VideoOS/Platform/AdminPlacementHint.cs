using System.Runtime.Serialization;

namespace VideoOS.Platform;

[DataContract]
public enum AdminPlacementHint
{
	[EnumMember(Value = "0")]
	Hidden,
	[EnumMember(Value = "1")]
	Default,
	[EnumMember(Value = "2")]
	Root
}
