namespace VideoOS.Platform.SDK.Interface;

public enum RestrictedMediaResultStatus
{
	Undefined,
	Success,
	Failed,
	PartlySuccess,
	RestrictedMediaNotFound,
	FeatureNotLicensed,
	RecorderNotFound,
	ManagementServerUnavailable,
	Unauthorized
}
