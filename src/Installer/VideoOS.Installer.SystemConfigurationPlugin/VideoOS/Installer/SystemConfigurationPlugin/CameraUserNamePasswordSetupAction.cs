using System.Collections.Generic;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class CameraUserNamePasswordSetupAction : PageSetupActionsBase
{
	public override string PageName => "CameraUserNamePasswordPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		return true;
	}
}
