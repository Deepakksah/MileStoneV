using System.Security.Principal;

namespace VideoOS.Management.Server;

public class IISHelper
{
	public enum IdentityType
	{
		MD_APPPOOL_IDENTITY_TYPE_LOCALSYSTEM,
		MD_APPPOOL_IDENTITY_TYPE_LOCALSERVICE,
		MD_APPPOOL_IDENTITY_TYPE_NETWORKSERVICE,
		MD_APPPOOL_IDENTITY_TYPE_SPECIFICUSER
	}

	public static void GetServiceAccountName(ref string accountName, ref IdentityType identityType)
	{
		FromCurrentIdentity(ref accountName, ref identityType);
	}

	public static void GetServiceAccountName(ref string accountName, ref SecurityIdentifier accountSid, ref IdentityType identityType)
	{
		FromCurrentIdentity(ref accountName, ref accountSid, ref identityType);
	}

	private static void FromCurrentIdentity(ref string accountName, ref SecurityIdentifier accountSid, ref IdentityType identityType)
	{
		WindowsIdentity current = WindowsIdentity.GetCurrent();
		if (current.User.IsWellKnown(WellKnownSidType.NetworkServiceSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_NETWORKSERVICE;
			accountSid = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
			accountName = accountSid.Translate(typeof(NTAccount)).ToString();
		}
		else if (current.User.IsWellKnown(WellKnownSidType.LocalServiceSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSERVICE;
			accountSid = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);
			accountName = accountSid.Translate(typeof(NTAccount)).ToString();
		}
		else if (current.User.IsWellKnown(WellKnownSidType.LocalSystemSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSYSTEM;
			accountSid = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
			accountName = accountSid.Translate(typeof(NTAccount)).ToString();
		}
		else
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_SPECIFICUSER;
			accountSid = current.User;
			accountName = accountSid.Translate(typeof(NTAccount)).ToString();
		}
	}

	private static void FromCurrentIdentity(ref string accountName, ref IdentityType identityType)
	{
		WindowsIdentity current = WindowsIdentity.GetCurrent();
		if (current.User.IsWellKnown(WellKnownSidType.NetworkServiceSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_NETWORKSERVICE;
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
			accountName = securityIdentifier.ToString();
		}
		else if (current.User.IsWellKnown(WellKnownSidType.LocalServiceSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSERVICE;
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null);
			accountName = securityIdentifier.ToString();
		}
		else if (current.User.IsWellKnown(WellKnownSidType.LocalSystemSid))
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_LOCALSYSTEM;
			SecurityIdentifier securityIdentifier = new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null);
			accountName = securityIdentifier.ToString();
		}
		else
		{
			identityType = IdentityType.MD_APPPOOL_IDENTITY_TYPE_SPECIFICUSER;
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			accountName = windowsPrincipal.Identity.Name;
		}
	}
}
