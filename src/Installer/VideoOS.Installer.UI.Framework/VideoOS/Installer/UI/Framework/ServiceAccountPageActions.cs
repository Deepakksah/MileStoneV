using System;
using System.Collections.Generic;
using System.ServiceProcess;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public class ServiceAccountPageActions : PageSetupActionsBase
{
	public override string PageName => "ServiceAccountPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		return base.BeforeActivatePage(parameters);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		base.BeforeLeavePage(parameters);
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (ValidateServiceAccountParameters(parameters))
		{
			dictionary = GlobalData.ToDict();
		}
		if (GlobalData.GetValueAsBoolean("USERACCOUNT") && dictionary.ContainsKey("INTERNALPASSWORD") && !string.IsNullOrEmpty(dictionary["INTERNALPASSWORD"]))
		{
			ServiceAccountErrorString serviceAccountErrorString = Account.ServiceAccountPasswordValidation(dictionary["SERVICEACCOUNT"], dictionary["INTERNALPASSWORD"]);
			if (serviceAccountErrorString != ServiceAccountErrorString.Empty)
			{
				ServiceAccountErrorHelper.ThrowErrorMessage(serviceAccountErrorString);
				return false;
			}
		}
		return true;
	}

	private bool ValidateServiceAccountParameters(Dictionary<string, string> parameters)
	{
		bool result = false;
		if (!GlobalData.HasParameter("SERVICEACCOUNT"))
		{
			if (!parameters.ContainsKey("SERVICEACCOUNT"))
			{
				throw new ArgumentNullException("Params.SERVICEACCOUNT");
			}
			GlobalData.SetValue("SERVICEACCOUNT", parameters["SERVICEACCOUNT"], "ServiceAccountPageActions.ValidateServParams()");
			result = true;
		}
		if (!GlobalData.HasParameter("SERVICEACCOUNT_NONLOC"))
		{
			if (parameters.ContainsKey("SERVICEACCOUNT_NONLOC"))
			{
				GlobalData.SetValue("SERVICEACCOUNT_NONLOC", parameters["SERVICEACCOUNT_NONLOC"], "ServiceAccountPageActions.ValidateServParams()");
				result = true;
			}
			else
			{
				ServiceAccountItem serviceAccountItem = new ServiceAccountItem(Resources.ServiceAccount_NetworkServiceName, ServiceAccount.NetworkService);
				string value = parameters["SERVICEACCOUNT"];
				string value2 = string.Empty;
				if (serviceAccountItem.AccountName.Equals(value))
				{
					value2 = serviceAccountItem.NonLocalisedAccountName;
				}
				else
				{
					serviceAccountItem = new ServiceAccountItem(Resources.ServiceAccount_LocalServiceName, ServiceAccount.LocalService);
					if (serviceAccountItem.AccountName.Equals(value))
					{
						value2 = serviceAccountItem.NonLocalisedAccountName;
					}
					else
					{
						serviceAccountItem = new ServiceAccountItem(Resources.ServiceAccount_LocalSystemName, ServiceAccount.LocalSystem);
						if (serviceAccountItem.AccountName.Equals(value))
						{
							value2 = serviceAccountItem.NonLocalisedAccountName;
						}
					}
				}
				GlobalData.SetValue("SERVICEACCOUNT_NONLOC", value2, "ServiceAccountPageActions.ValidateServParams()");
			}
		}
		if (!GlobalData.HasParameter("USERACCOUNT") && parameters.ContainsKey("USERACCOUNT"))
		{
			GlobalData.SetValue("USERACCOUNT", "false", "ServiceAccountPageActions.ValidateServParams()");
			result = true;
		}
		if (GlobalData.HasParameter("ENCRYPTEDPASSWORD"))
		{
			string value3 = string.Empty;
			if (GlobalData.TryGetValue("ENCRYPTEDPASSWORD", out value3))
			{
				if (string.IsNullOrEmpty(value3))
				{
					GlobalData.RemoveValue("ENCRYPTEDPASSWORD");
				}
			}
			else
			{
				GlobalData.RemoveValue("ENCRYPTEDPASSWORD");
			}
		}
		return result;
	}
}
