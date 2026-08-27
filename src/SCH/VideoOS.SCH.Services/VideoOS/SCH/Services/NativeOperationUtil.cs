using System.Security.Principal;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services;

public class NativeOperationUtil
{
	public static bool IsAnonymous => GetClientInfo().IsAnonymous;

	public static ClientSecurityInfo GetClientInfo()
	{
		ClientSecurityInfo clientSecurityInfo = new ClientSecurityInfo();
		if (!WindowsIdentity.GetCurrent().IsAnonymous && !WindowsIdentity.GetCurrent().IsGuest && !WindowsIdentity.GetCurrent().IsSystem && WindowsIdentity.GetCurrent().IsAuthenticated)
		{
			clientSecurityInfo.IsAnonymous = false;
			IdentityUtil.ExtractUserNameAndDomain(WindowsIdentity.GetCurrent().Name, out var userName, out var domain);
			clientSecurityInfo.UserName = userName;
			clientSecurityInfo.UserDomain = domain;
			clientSecurityInfo.AuthenticationType = ClientAuthenticationType.Windows;
		}
		return clientSecurityInfo;
	}

	public static string GetClientUserName()
	{
		return GetClientInfo().UserName;
	}

	public static string GetClientUserDomain()
	{
		return GetClientInfo().UserDomain;
	}

	public static ClientAuthenticationType GetClientAuthenticationType()
	{
		return GetClientInfo().AuthenticationType;
	}
}
