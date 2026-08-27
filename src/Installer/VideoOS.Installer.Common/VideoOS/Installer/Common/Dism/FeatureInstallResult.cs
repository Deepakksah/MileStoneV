namespace VideoOS.Installer.Common.Dism;

public class FeatureInstallResult
{
	public InstallStatus Status { get; private set; }

	public string FeatureName { get; private set; }

	public string ErrorMessage { get; private set; }

	public FeatureInstallResult(InstallStatus status, string failedFeature, string errorMessage)
	{
		Status = status;
		FeatureName = failedFeature;
		ErrorMessage = errorMessage;
	}
}
