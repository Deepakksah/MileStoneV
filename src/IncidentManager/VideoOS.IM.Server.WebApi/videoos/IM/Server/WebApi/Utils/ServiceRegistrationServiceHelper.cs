using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace VideoOS.IM.Server.WebApi.Utils;

public class ServiceRegistrationServiceHelper : WcfServiceHelper
{
	public static ServiceRegistrationServiceClient CreateClient(Uri managementServerAddress, string accessToken)
	{
		Uri uri = new Uri(managementServerAddress, "ManagementServer/ServiceRegistrationServiceOAuth.svc");
		Binding oAuthBinding = WcfServiceHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps);
		EndpointAddress endpointAddress = WcfServiceHelper.GetEndpointAddress(uri);
		return new ServiceRegistrationServiceClient(oAuthBinding, endpointAddress)
		{
			Endpoint = 
			{
				Behaviors = { (IEndpointBehavior)new AddTokenBehavior(accessToken) }
			}
		};
	}
}
