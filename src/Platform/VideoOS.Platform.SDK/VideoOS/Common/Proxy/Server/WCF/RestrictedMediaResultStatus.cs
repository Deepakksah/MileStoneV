using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace VideoOS.Common.Proxy.Server.WCF;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RestrictedMediaResultStatus", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.WebService.Common.ServerCommandService")]
public enum RestrictedMediaResultStatus
{
	[EnumMember]
	Undefined,
	[EnumMember]
	Success,
	[EnumMember]
	Failed,
	[EnumMember]
	PartlySuccess,
	[EnumMember]
	RestrictedMediaNotFound,
	[EnumMember]
	FeatureNotLicensed,
	[EnumMember]
	RecorderNotFound,
	[EnumMember]
	ManagementServerUnavailable,
	[EnumMember]
	Unauthorized
}
