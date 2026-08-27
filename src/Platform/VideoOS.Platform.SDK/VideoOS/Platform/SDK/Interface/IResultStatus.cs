namespace VideoOS.Platform.SDK.Interface;

public enum IResultStatus
{
	Undefined,
	Success,
	Failed,
	PartlySuccess,
	MarkedDataNotFound,
	FeatureNotLicensed,
	RecorderNotFound,
	ManagementServerUnavailable,
	Unauthorized
}
