using System;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public static class ServiceAccountErrorHelper
{
	public static void ThrowErrorMessage(ServiceAccountErrorString errorString)
	{
		if (errorString != ServiceAccountErrorString.Empty)
		{
			SetErrorMessage(errorString);
		}
	}

	private static void SetErrorMessage(ServiceAccountErrorString errorString)
	{
		string arg = string.Empty;
		switch (errorString)
		{
		case ServiceAccountErrorString.AccessDenied:
			arg = Resources.ServiceAccount_MessageAccessDenied;
			break;
		case ServiceAccountErrorString.AddRightGrantedFailed:
			arg = Resources.ServiceAccount_MessageAddAcountRightFailed;
			break;
		case ServiceAccountErrorString.NameNotFound:
			arg = Resources.ServiceAccount_MessageNameNotFound;
			break;
		case ServiceAccountErrorString.PasswordInvalid:
			arg = Resources.ServiceAccount_MessagePasswordInvalid;
			break;
		case ServiceAccountErrorString.Unknown:
			arg = Resources.ServiceAccount_MessageUnknownError;
			break;
		}
		throw new UIFrameworkErrorException($"{Resources.ServiceAccount_MessageAccountLogOnFailed + Environment.NewLine}{arg}", enablePreviousButtong: true);
	}
}
