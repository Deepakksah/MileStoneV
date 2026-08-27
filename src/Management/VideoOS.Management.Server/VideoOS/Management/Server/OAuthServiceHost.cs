using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.IdentityServer.Common.ServiceModel;

namespace VideoOS.Management.Server;

public class OAuthServiceHost : ServiceHost
{
	public OAuthServiceHost(Type serviceType, params Uri[] baseAddresses)
		: base(serviceType, baseAddresses)
	{
	}

	protected override void InitializeRuntime()
	{
		Uri idpAddress = new Uri(Config.Instance.GetAppSetting("AuthorizationServerUri", "http://localhost:80/IDP"));
		base.Credentials.AddOpenIdAuthentication(idpAddress, new List<string>());
		base.InitializeRuntime();
	}
}
