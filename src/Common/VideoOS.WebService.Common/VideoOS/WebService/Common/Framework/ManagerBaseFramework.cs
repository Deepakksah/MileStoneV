using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.WebService.Common.Framework;

public abstract class ManagerBaseFramework<TCommunicationObject, TChannel> : ManagerBase<TCommunicationObject, TChannel> where TCommunicationObject : ICommunicationObject where TChannel : class
{
	protected ManagerBaseFramework(Uri serverUrl, string namespaceName, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, tokenCache, exceptionHandler)
	{
	}

	protected ManagerBaseFramework(Uri serverUrl, string namespaceName, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, exceptionHandler)
	{
		base.AuthenticationType = ServerAuthentication.LegacyWindowsCurrentUser;
	}

	protected ManagerBaseFramework(Uri serverUrl, string namespaceName, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, exceptionHandler)
	{
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		base.Password = password ?? throw new ArgumentNullException("password");
		if (domain == null)
		{
			throw new ArgumentNullException("domain");
		}
		base.AuthenticationType = ServerAuthentication.LegacyWindows;
		base.UserName = FormattableString.Invariant($"{domain}\\{userName}");
	}

	protected ManagerBaseFramework(Uri serverUrl, string namespaceName, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
		: base(serverUrl, namespaceName, exceptionHandler)
	{
		base.UserName = userName ?? throw new ArgumentNullException("userName");
		base.Password = password ?? throw new ArgumentNullException("password");
		base.AuthenticationType = ServerAuthentication.LegacyBasic;
	}

	public void ConfigureCredentials(ClientCredentials clientCredentials)
	{
		ManagementServerWcfHelper.ConfigureCredentials(base.AuthenticationType, clientCredentials, base.UserName, base.Password);
	}
}
