namespace VideoOS.Common.Integration;

public class TestFeatureManager : FeatureManager
{
	public bool FeatureLicensed { get; set; }

	public override bool IsFeatureLicensed(string featureName)
	{
		return FeatureLicensed;
	}
}
