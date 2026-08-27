using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Util.Svc;

public class ServiceRegistrationApiSvcOauthDecorator : ServiceRegistrationApiSvcDecorator
{
	private readonly LoginSettings _loginSettings;

	public override bool IsBasicUser => _loginSettings.IsBasicUser;

	public override ICredentials Credentials => _loginSettings.NetworkCredential;

	public ServiceRegistrationApiSvcOauthDecorator(LoginSettings loginSettings)
	{
		_loginSettings = loginSettings;
	}

	public override ServiceRegistrationServiceClient GetClient(Uri url, ICredentials credentials, bool isBasicUser)
	{
		Binding oAuthBinding = ServiceApiSvcHelper.GetOAuthBinding(url.Scheme == Uri.UriSchemeHttps);
		string spn = SpnFactory.GetSpn(url);
		EndpointAddress remoteAddress = new EndpointAddress(url, new SpnEndpointIdentity(spn));
		ServiceRegistrationServiceClient obj = new ServiceRegistrationServiceClient(oAuthBinding, remoteAddress)
		{
			Endpoint = 
			{
				EndpointBehaviors = { (IEndpointBehavior)new ServiceUtil.AddTokenBehavior(_loginSettings) }
			}
		};
		ServiceApiSvcHelper.ConfigureEndpoint(obj.Endpoint);
		return obj;
	}
}
