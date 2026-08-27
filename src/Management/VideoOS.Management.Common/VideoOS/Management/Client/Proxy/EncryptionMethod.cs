using System.Runtime.Serialization;

namespace VideoOS.Management.Client.Proxy;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Server")]
public enum EncryptionMethod
{
	[EnumMember]
	None,
	[EnumMember]
	Light,
	[EnumMember]
	Strong
}
