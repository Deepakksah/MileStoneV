using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.WebService.Common.Framework.Factory;

public abstract class ProxyClientFactoryBase
{
	public static T CreateService<T>(Uri serverUrl, string namespaceName, ITokenCache tokenCache)
	{
		return CreateService<T>(serverUrl, namespaceName, tokenCache, TimeSpan.FromMinutes(1.0));
	}

	public static T CreateService<T>(Uri serverUrl, string namespaceName, ITokenCache tokenCache, TimeSpan timeout)
	{
		return CreateServiceOAuth<T>(serverUrl, namespaceName, tokenCache, (serverUrl.Scheme == Uri.UriSchemeHttps) ? ServerAuthentication.OAuthSecure : ServerAuthentication.OAuthUnsecure, null, null, timeout);
	}

	private static T CreateServiceOAuth<T>(Uri serverUrl, string namespaceName, ITokenCache tokenCache, ServerAuthentication authenticationType, string userName, string password, TimeSpan timeout)
	{
		Binding binding = ManagementServerWcfHelper.GetBinding(authenticationType, namespaceName, timeout);
		EndpointAddress endpointAddress = ManagementServerWcfHelper.GetEndpointAddress(authenticationType, serverUrl);
		ChannelFactory<T> channelFactory = new ChannelFactory<T>(binding, endpointAddress);
		ManagementServerWcfHelper.ConfigureEndpoint(authenticationType, channelFactory.Endpoint, tokenCache);
		return channelFactory.CreateChannel();
	}
}
