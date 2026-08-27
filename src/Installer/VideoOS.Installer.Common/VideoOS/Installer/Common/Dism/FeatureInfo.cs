namespace VideoOS.Installer.Common.Dism;

public class FeatureInfo
{
	public string FeatureName { get; private set; }

	public PackageFeatureState FeatureState { get; private set; }

	public string DisplayName { get; private set; }

	public string Description { get; private set; }

	public RestartType RestartRequired { get; private set; }

	private FeatureInfo()
	{
	}

	public FeatureInfo(string featureName, PackageFeatureState featureState, string displayName, string description, RestartType restartRequired)
	{
		FeatureName = featureName;
		FeatureState = featureState;
		DisplayName = displayName;
		Description = description;
		RestartRequired = restartRequired;
	}
}
