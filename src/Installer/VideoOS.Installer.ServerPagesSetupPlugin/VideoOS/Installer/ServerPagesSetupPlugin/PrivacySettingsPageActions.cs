using System;
using System.Collections.Generic;
using VideoOS.Installer.UI.Framework;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class PrivacySettingsPageActions : PageSetupActionsBase
{
	public override string PageName => "PrivacySettingsPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (OemResources.Instance.App.IsOem)
		{
			return false;
		}
		if (parameters.ContainsKey("HARDWARE-MANUFACTURER-MODE") && bool.Parse(parameters["HARDWARE-MANUFACTURER-MODE"]))
		{
			return false;
		}
		return base.BeforeActivatePage(parameters);
	}
}
