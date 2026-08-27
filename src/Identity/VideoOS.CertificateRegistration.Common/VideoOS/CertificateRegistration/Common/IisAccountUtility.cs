using System;
using System.Net.NetworkInformation;
using System.Security.Principal;
using Microsoft.Web.Administration;

namespace VideoOS.CertificateRegistration.Common;

public class IisAccountUtility : IIisAccountUtility
{
	public string GetAccountUser(string appPoolName)
	{
		ApplicationPool appPool = new ServerManager().ApplicationPools[appPoolName];
		return GetAccountUser(appPool);
	}

	private string GetAccountUser(ApplicationPool appPool)
	{
		ProcessModelIdentityType identityType = appPool.ProcessModel.IdentityType;
		return identityType switch
		{
			ProcessModelIdentityType.LocalSystem => AccountUserfromSidType(WellKnownSidType.LocalSystemSid), 
			ProcessModelIdentityType.LocalService => AccountUserfromSidType(WellKnownSidType.LocalServiceSid), 
			ProcessModelIdentityType.NetworkService => AccountUserfromSidType(WellKnownSidType.NetworkServiceSid), 
			ProcessModelIdentityType.SpecificUser => AccountUserfromDomainOrWorkgroup(appPool.ProcessModel.UserName), 
			ProcessModelIdentityType.ApplicationPoolIdentity => "IIS AppPool\\" + appPool.Name, 
			_ => throw new ArgumentOutOfRangeException("identityType", identityType, "Unexpected value"), 
		};
	}

	private string AccountUserfromSidType(WellKnownSidType localSystemSid)
	{
		return new SecurityIdentifier(localSystemSid, null).Translate(typeof(NTAccount)).Value;
	}

	private string AccountUserfromDomainOrWorkgroup(string userName)
	{
		string result = userName;
		IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
		if (string.IsNullOrEmpty(iPGlobalProperties.DomainName))
		{
			string[] array = userName.Split('\\');
			if (array.Length > 1)
			{
				result = iPGlobalProperties.HostName + "\\" + array[1];
			}
		}
		return result;
	}
}
