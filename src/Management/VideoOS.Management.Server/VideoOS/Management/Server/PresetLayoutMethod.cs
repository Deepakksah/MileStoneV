using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "PresetLayoutMethodInfo")]
public enum PresetLayoutMethod
{
	[EnumMember]
	Preserve,
	[EnumMember]
	Clear
}
