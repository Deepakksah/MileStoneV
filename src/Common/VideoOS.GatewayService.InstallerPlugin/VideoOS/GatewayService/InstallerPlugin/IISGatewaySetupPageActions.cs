using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.IISSetup;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.InstallerPlugin;

public class IISGatewaySetupPageActions : IISSetupPageActions
{
	public string Nope { get; set; }

	public override string PageName => "IISSetupPage";

	public IISGatewaySetupPageActions()
	{
	}

	public IISGatewaySetupPageActions(string nope)
	{
		Nope = nope;
	}

	protected override SiteInfo SelectWebsiteToUse()
	{
		if (GlobalData.TryGetValue("MANAGEMENTSERVERURI", out var value) && value.StartsWith("https"))
		{
			SetConfiguredIisWebsites();
		}
		return base.SelectWebsiteToUse();
	}

	private void SetConfiguredIisWebsites()
	{
		IisWebsite[] iisWebsites = IisHelper.GetIisWebsites(LogManager.InnerLog);
		string text = string.Empty;
		IisWebsite[] array = iisWebsites;
		foreach (IisWebsite iisWebsite in array)
		{
			if (iisWebsite.Status == IisWebsiteStatus.Valid)
			{
				text = text + iisWebsite.Id + ";";
			}
		}
		GlobalData.SetValue("ConfiguredSitesIds", text, "SetupIIS.SelectWebsiteToUse()");
	}
}
