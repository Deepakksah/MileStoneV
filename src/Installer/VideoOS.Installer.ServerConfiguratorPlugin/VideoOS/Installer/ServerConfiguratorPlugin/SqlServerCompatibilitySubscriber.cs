using System;
using System.Collections.Generic;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.ServerConfiguratorPlugin.Properties;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class SqlServerCompatibilitySubscriber : IWizardCommonPageSubscription<WarningPageContent>
{
	public Guid Id => new Guid("1254A656-26CB-4605-8B4E-4E873AE56DFF");

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		bool num = parameters != null && parameters.ContainsKey("IS_UPGRADE") && parameters["IS_UPGRADE"] == "true";
		bool isModify = InstallerManager.Instance.IsModify;
		if (num || isModify)
		{
			return false;
		}
		if (CommonHelper.IsWindows8Or2012OrBelow())
		{
			LogManager.Write("New installation on Windows 2012 or Windows 8 detected where SQL Server 2019 is not supported");
			return true;
		}
		return false;
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return null;
	}

	public WarningPageContent GetContent()
	{
		Uri uri = new Uri(OemResources.Instance.App.SystemRequirementsUrl);
		ContentLinkText details = new ContentLinkText(Resources.WarningPageBodyDetails, uri);
		ContentLinkText subsection = new ContentLinkText(Resources.WarningPageBodySubsection);
		return new WarningPageContent(Resources.WarningPageHeadingDetails, details, Resources.WarningPageHeadingSubsection, subsection, 20);
	}
}
