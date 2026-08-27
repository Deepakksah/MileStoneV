using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework.PageActions;

public abstract class ServerSelectionPageActionsBase : PageSetupActionsBase
{
	private ICredentials _credentials = (NetworkCredential)CredentialCache.DefaultCredentials;

	protected void SetLoginCheckCredentials(ICredentials credentials)
	{
		_credentials = credentials;
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if ((parameters.ContainsKey("ISCORPORATEINSTALLER") && parameters["ISCORPORATEINSTALLER"] == "true") || !GlobalData.GetValueAsBoolean("QUIET-MODE"))
		{
			return base.BeforeLeavePage(parameters);
		}
		if (parameters.ContainsKey("MANAGEMENTSERVERURI") && !string.IsNullOrEmpty(parameters["MANAGEMENTSERVERURI"]))
		{
			Uri validatedAddress = null;
			if (!HostHelper.ValidateAddress(parameters["MANAGEMENTSERVERURI"], out validatedAddress))
			{
				throw new InvalidOperationException(Resources.ServerSelection_AddressNotValid);
			}
			if (new ServerHelper().TryGetManagementServerInfo(validatedAddress, out var managementServerInfo) && ServerHelper.TryLoginToCorporate(validatedAddress, managementServerInfo.IDPAddress, _credentials))
			{
				GlobalData.SetValue("MANAGEMENTSERVERURI", validatedAddress.AbsoluteUri, "ServerSelectionPageActions.BeforeLeavePage()");
				GlobalData.SetValue("MANAGEMENTAPPNAME", managementServerInfo.HttpAddress.AbsoluteUri, "ServerSelectionPageActions.BeforeLeavePage()");
				GlobalData.SetValue("MANAGEMENTAPPNAMESECURE", managementServerInfo.HttpsAddress.AbsoluteUri, "ServerSelectionPageActions.BeforeLeavePage()");
				GlobalData.SetValue("IDPADDRESS", managementServerInfo.IDPAddress.AbsoluteUri, "ServerSelectionPageActions.BeforeLeavePage()");
				return base.BeforeLeavePage(parameters);
			}
			throw new InvalidOperationException(Resources.ServerSelection_ServerNotFound);
		}
		throw new InvalidOperationException("Argument MANAGEMENTSERVERURI is not defined.");
	}
}
