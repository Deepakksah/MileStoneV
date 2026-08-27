using System.Collections.Generic;
using VideoOS.Installer.Common;

namespace VideoOS.Installer.UI.Framework;

public class TrecksIntersectionPageActions : PageSetupActionsBase
{
	public override string PageName => "TreckIntersectionPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") || GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			return false;
		}
		return base.BeforeActivatePage(parameters);
	}
}
