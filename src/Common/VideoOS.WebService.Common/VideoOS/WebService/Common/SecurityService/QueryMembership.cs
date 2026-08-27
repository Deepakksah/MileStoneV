using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public enum QueryMembership
{
	[EnumMember]
	None,
	[EnumMember]
	Direct,
	[EnumMember]
	Expanded
}
