using System;
using System.Collections.Generic;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.UI.Framework.PageActions;

public class WelcomePageActions : PageSetupActionsBase
{
	public override string PageName => "WelcomePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if ((GlobalData.TryGetValue("UPDATE_TO_PRODUCTNAME", out var _) && GlobalData.TryGetValue("LICENSE", out var value2)) || (GlobalData.TryGetValue("LICENSE", out value2) && GlobalData.TryGetValue("UPDATE_LECENSE_NOT_REQUIRED", out var value3) && value3.Equals("true", StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}
		return true;
	}
}
