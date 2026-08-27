using System.Collections.Generic;
using VideoOS.Installer.Configuration;

namespace VideoOS.Installer.Manager;

public class SubscriptionBypassFilterInModify : SubscriptionBypassFilter
{
	protected override IEnumerable<string> PackageIdsToBeInstalled => InstallerManager.Instance.SequenceManager.PackagesToInstall;

	public SubscriptionBypassFilterInModify(InstallerConfig installerConfig)
		: base(installerConfig)
	{
	}
}
