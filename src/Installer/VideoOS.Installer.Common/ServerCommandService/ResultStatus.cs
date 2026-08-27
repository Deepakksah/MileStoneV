using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace ServerCommandService;

[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ResultStatus", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.WebService.Common.ServerCommandService")]
public enum ResultStatus
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
	MarkedDataNotFound,
	[EnumMember]
	FeatureNotLicensed,
	[EnumMember]
	RecorderNotFound,
	[EnumMember]
	ManagementServerUnavailable,
	[EnumMember]
	Unauthorized
}
