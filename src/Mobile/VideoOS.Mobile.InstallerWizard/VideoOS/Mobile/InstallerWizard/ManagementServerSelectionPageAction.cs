using System.Collections.Generic;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework.PageActions;

namespace VideoOS.Mobile.InstallerWizard;

public class ManagementServerSelectionPageAction : ServerSelectionPageActionsBase
{
	public override string PageName => "ServerPage";

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		bool result = default(bool);
		if (parameters.TryGetValue("USERACCOUNT", out var value) && bool.TryParse(value, out result) && result)
		{
			parameters.TryGetValue("SERVICEACCOUNT", out var value2);
			parameters.TryGetValue("ENCRYPTEDPASSWORD", out var value3);
			SetLoginCheckCredentials(ServerHelper.ResolveCredentials(value2, EncryptHelper.DecryptPassword(value3, unicodePassword: true)));
		}
		return base.BeforeLeavePage(parameters);
	}
}
