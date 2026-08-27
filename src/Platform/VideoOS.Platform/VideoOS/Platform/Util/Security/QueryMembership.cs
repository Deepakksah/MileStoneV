using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Util.Security;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "QueryMembership", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public enum QueryMembership
{
	[EnumMember]
	None,
	[EnumMember]
	Direct,
	[EnumMember]
	Expanded
}
