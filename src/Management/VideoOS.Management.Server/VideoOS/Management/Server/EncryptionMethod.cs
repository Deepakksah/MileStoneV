using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "EncryptionMethod")]
public enum EncryptionMethod
{
	[EnumMember]
	None,
	[EnumMember]
	Light,
	[EnumMember]
	Strong
}
