using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

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
