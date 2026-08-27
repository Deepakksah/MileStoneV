using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "ElementInsertionMethodInfo")]
public enum ElementInsertionMethod
{
	[EnumMember]
	Linked,
	[EnumMember]
	Independent
}
