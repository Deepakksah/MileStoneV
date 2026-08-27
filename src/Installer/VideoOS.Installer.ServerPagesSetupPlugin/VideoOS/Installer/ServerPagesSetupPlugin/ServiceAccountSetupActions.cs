using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Principal;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class ServiceAccountSetupActions : ServiceAccountPageActions
{
	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		return base.BeforeActivatePage(parameters);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		if (!base.BeforeLeavePage(parameters))
		{
			return false;
		}
		if (parameters == null)
		{
			return false;
		}
		if (parameters.ContainsKey("INTERNALPASSWORD"))
		{
			using (new ShowWaitCursor())
			{
				ServiceAccountErrorString serviceAccountErrorString = ServiceAccountErrorString.Empty;
				if (!Account.HasAccountAccountRight(string.Empty, parameters["SERVICEACCOUNT"], "SeInteractiveLogonRight"))
				{
					serviceAccountErrorString = Account.SetServiceAccountRights("SeInteractiveLogonRight", parameters["SERVICEACCOUNT"], parameters["INTERNALPASSWORD"]);
				}
				if (serviceAccountErrorString != ServiceAccountErrorString.Empty)
				{
					return false;
				}
				if (!Account.HasAccountAccountRight(string.Empty, parameters["SERVICEACCOUNT"], "SeServiceLogonRight"))
				{
					serviceAccountErrorString = Account.SetServiceAccountRights("SeServiceLogonRight", parameters["SERVICEACCOUNT"], parameters["INTERNALPASSWORD"]);
				}
				if (serviceAccountErrorString != ServiceAccountErrorString.Empty)
				{
					return false;
				}
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, Resources.Logging_ServiceAccountAddRightGranted, parameters["SERVICEACCOUNT"]), TraceEventType.Information);
			}
		}
		return true;
	}

	public static bool IsUserAdministrator(string userName)
	{
		return Account.IsUserAdministrator(userName);
	}

	public static bool IsCurrentUserAdministrator()
	{
		using WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
		return IsUserAdministrator(windowsIdentity.Name);
	}

	public static bool IsServiceAccountValid(string userName, string domain, string password)
	{
		return Account.IsServiceAccountValid(userName, domain, password);
	}
}
