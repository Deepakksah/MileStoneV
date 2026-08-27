using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services;

public class ServiceOperationUtil
{
	public static bool IsAnonymous => GetClientInfo().IsAnonymous;

	private static ServiceSecurityContext GetCurrSecurityContext()
	{
		if (OperationContext.Current != null && OperationContext.Current.ServiceSecurityContext != null)
		{
			return OperationContext.Current.ServiceSecurityContext;
		}
		return null;
	}

	private static RequestContext GetCurrRequestContext()
	{
		if (OperationContext.Current != null && OperationContext.Current.RequestContext != null)
		{
			return OperationContext.Current.RequestContext;
		}
		return null;
	}

	private static string GetFirstNameFromClaimSet(AuthorizationContext auth, ClaimSet csType)
	{
		if (auth != null)
		{
			foreach (ClaimSet claimSet in auth.ClaimSets)
			{
				IEnumerable<Claim> source = claimSet.FindClaims("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", Rights.PossessProperty);
				if (source.Count() > 0)
				{
					return source.First().Resource.ToString();
				}
			}
		}
		return null;
	}

	public static ClientSecurityInfo GetClientInfo()
	{
		ClientSecurityInfo clientSecurityInfo = new ClientSecurityInfo();
		ServiceSecurityContext currSecurityContext = GetCurrSecurityContext();
		if (currSecurityContext != null)
		{
			string userName;
			string domain;
			if (currSecurityContext.IsAnonymous)
			{
				if (currSecurityContext.AuthorizationContext != null)
				{
					string firstNameFromClaimSet = GetFirstNameFromClaimSet(currSecurityContext.AuthorizationContext, ClaimSet.System);
					if (!string.IsNullOrEmpty(firstNameFromClaimSet))
					{
						clientSecurityInfo.IsAnonymous = false;
						clientSecurityInfo.UserName = firstNameFromClaimSet;
						clientSecurityInfo.UserDomain = null;
						clientSecurityInfo.AuthenticationType = ClientAuthenticationType.UserNamePassword;
					}
				}
			}
			else if (currSecurityContext.WindowsIdentity != null)
			{
				if (!string.IsNullOrEmpty(currSecurityContext.WindowsIdentity.Name))
				{
					clientSecurityInfo.IsAnonymous = false;
					IdentityUtil.ExtractUserNameAndDomain(currSecurityContext.WindowsIdentity.Name, out userName, out domain);
					if (userName == "ANONYMOUS LOGON")
					{
						clientSecurityInfo.IsAnonymous = true;
					}
					clientSecurityInfo.UserName = userName;
					clientSecurityInfo.UserDomain = domain;
					clientSecurityInfo.AuthenticationType = ClientAuthenticationType.Windows;
				}
			}
			else if (currSecurityContext.PrimaryIdentity != null && !string.IsNullOrEmpty(currSecurityContext.PrimaryIdentity.Name))
			{
				clientSecurityInfo.IsAnonymous = false;
				IdentityUtil.ExtractUserNameAndDomain(currSecurityContext.WindowsIdentity.Name, out userName, out domain);
				clientSecurityInfo.UserName = userName;
				clientSecurityInfo.UserDomain = domain;
				clientSecurityInfo.AuthenticationType = ClientAuthenticationType.Certificate;
			}
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

	public static string GetSoapAction()
	{
		RequestContext currRequestContext = GetCurrRequestContext();
		if (currRequestContext != null && currRequestContext.RequestMessage != null)
		{
			return currRequestContext.RequestMessage.Headers.Action;
		}
		return null;
	}
}
