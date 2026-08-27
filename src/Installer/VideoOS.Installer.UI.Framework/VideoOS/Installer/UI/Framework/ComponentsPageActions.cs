using System.Collections.Generic;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.UI.Framework;

public class ComponentsPageActions : PageSetupActionsBase
{
	public override string PageName => "ComponentPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") || GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			return false;
		}
		return base.BeforeActivatePage(parameters);
	}
}
