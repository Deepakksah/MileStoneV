using System.Collections.Generic;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class LicensePageActions : PageSetupActionsBase
{
	public override string PageName => "LicensePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return false;
		}
		return base.BeforeActivatePage(parameters);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return base.BeforeLeavePage(parameters);
		}
		if (GlobalData.GetValueAsBoolean("QUIET-MODE") && !parameters.ContainsKey("LICENSE"))
		{
			new LicenseInstallerManager().ValidateLicenseStatus(LicenseStatus.Invalid);
			return false;
		}
		return base.BeforeLeavePage(parameters);
	}
}
