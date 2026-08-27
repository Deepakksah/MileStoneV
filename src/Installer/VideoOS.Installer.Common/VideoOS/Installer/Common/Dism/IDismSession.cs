using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Common.Dism;

public interface IDismSession
{
	event EventHandler<ProgressInfoEventArgs> Progress;

	void Open();

	DismStatus Close();

	IEnumerable<Feature> QueryFeatures();

	FeatureInfo QueryFeatureInfo(string featureName);

	DismStatus EnableFeature(string featureName, bool enableAll);

	DismStatus DisableFeature(string featureName);

	FeatureInstallResult EnsureFeaturesAreInstalled(params string[] featureNamesToEnsure);

	DismStatus Shutdown();
}
