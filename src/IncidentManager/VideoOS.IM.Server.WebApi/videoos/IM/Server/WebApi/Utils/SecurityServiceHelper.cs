using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace VideoOS.IM.Server.WebApi.Utils;

public class SecurityServiceHelper : WcfServiceHelper
{
	public static SecurityServiceClient CreateClient(Uri managementServerAddress, string accessToken)
	{
		Uri uri = new Uri(managementServerAddress, "ManagementServer/SecurityServiceOAuth.svc");
		Binding oAuthBinding = WcfServiceHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps);
		EndpointAddress endpointAddress = WcfServiceHelper.GetEndpointAddress(uri);
		return new SecurityServiceClient(oAuthBinding, endpointAddress)
		{
			Endpoint = 
			{
				Behaviors = { (IEndpointBehavior)new AddTokenBehavior(accessToken) }
			}
		};
	}
}
