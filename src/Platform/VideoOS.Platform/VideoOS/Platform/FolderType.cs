using System.Runtime.Serialization;

namespace VideoOS.Platform;

[DataContract]
public enum FolderType
{
	[EnumMember(Value = "0")]
	No,
	[EnumMember(Value = "1")]
	SystemDefined,
	[EnumMember(Value = "2")]
	UserDefined
}
