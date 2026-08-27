using System.Collections.Generic;
using VideoOS.Installer.Common;
using VideoOS.Installer.DatabasePlugin;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class DbInstallSetupPageActions : DbInstallPageActions
{
	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		base.BeforeActivatePage(parameters);
		if (GlobalData.TryGetValue("INSTALLATION_MODE", out var value) && (value == "Typical" || value == "HradwareMode"))
		{
			return false;
		}
		return true;
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		base.BeforeLeavePage(parameters);
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (!dictionary.ContainsKey("SQL-SERVER-LOCAL"))
		{
			GlobalData.SetValue("SQL-SERVER-LOCAL", (dictionary["InstallNewServerChecked"] == "1") ? "yes" : "no", "DbInstallSetupPageActions.BeforeLeavePage()");
			dictionary = GlobalData.ToDict();
		}
		return true;
	}
}
