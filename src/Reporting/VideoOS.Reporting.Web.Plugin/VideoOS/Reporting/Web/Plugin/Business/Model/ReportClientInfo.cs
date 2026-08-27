using System;
using VideoOS.Reporting.Web.Plugin.Base;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public class ReportClientInfo
{
	public ClientAuthenticationType AuthenticationType { get; set; }

	public bool IsAnonymous { get; set; }

	public IdentityTokenInfo IdentityToken { get; set; }

	public string FullUserName { get; set; }

	public string UserName { get; set; }

	public string UserDomain { get; set; }

	public string Sid { get; set; }

	public ReportClientInfo(ClientAuthenticationType authenticationType, bool isAnonymous)
	{
		AuthenticationType = authenticationType;
		IsAnonymous = isAnonymous;
		IdentityToken = null;
		FullUserName = null;
		UserName = null;
		UserDomain = null;
	}

	public static ClientAuthenticationType GetClientAuthenticationType(string authenticationType)
	{
		if (EnumUtility.IsDefined(typeof(ClientAuthenticationType), authenticationType))
		{
			return (ClientAuthenticationType)Enum.Parse(typeof(ClientAuthenticationType), authenticationType, ignoreCase: true);
		}
		return ClientAuthenticationType.None;
	}
}
