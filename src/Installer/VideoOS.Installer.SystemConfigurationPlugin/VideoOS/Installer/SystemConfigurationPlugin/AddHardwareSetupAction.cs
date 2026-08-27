using System;
using System.Collections.Generic;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddHardwareSetupAction : PageSetupActionsBase
{
	public override string PageName => "AddHardwarePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		string value = "";
		if (parameters.TryGetValue(ServerParameters.ScannedHardware.ToString(), out value) && HardwareConfigurations.IsEmpty(value))
		{
			return false;
		}
		return true;
	}
}
