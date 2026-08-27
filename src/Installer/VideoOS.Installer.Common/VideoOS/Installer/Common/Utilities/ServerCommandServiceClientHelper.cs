using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using ServerCommandService;

namespace VideoOS.Installer.Common.Utilities;

public class ServerCommandServiceClientHelper : WcfServiceHelper
{
	public static ServerCommandServiceClient CreateClient(Uri managementServerAddress, string accessToken)
	{
		Uri uri = new Uri(managementServerAddress, "ManagementServer/ServerCommandServiceOAuth.svc");
		Binding oAuthBinding = WcfServiceHelper.GetOAuthBinding(uri.Scheme == Uri.UriSchemeHttps);
		EndpointAddress endpointAddress = WcfServiceHelper.GetEndpointAddress(uri);
		return new ServerCommandServiceClient(oAuthBinding, endpointAddress)
		{
			Endpoint = 
			{
				Behaviors = { (IEndpointBehavior)new AddTokenBehavior(accessToken) }
			}
		};
	}
}
