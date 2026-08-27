using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "EnableFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public enum EnableFilter
{
	[EnumMember]
	All,
	[EnumMember]
	Enabled,
	[EnumMember]
	Disabled
}
