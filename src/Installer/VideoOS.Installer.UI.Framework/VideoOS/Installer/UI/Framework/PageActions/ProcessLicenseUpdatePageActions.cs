using System.Collections.Generic;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.UI.Framework.PageActions;

public class ProcessLicenseUpdatePageActions : PageSetupActionsBase
{
	public override string PageName => "ProcessLicenseUpdatePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (!GlobalData.TryGetValue("UPDATE_TO_PRODUCTNAME", out var value) || string.IsNullOrEmpty(value))
		{
			return false;
		}
		return true;
	}
}
