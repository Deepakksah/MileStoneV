using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.ServiceModel;

namespace VideoOS.WebService.Common.Framework;

public class ManagementServerWcfHelperBase
{
	public static Binding GetBinding(ServerAuthentication serverAuthentication, string namespaceName)
	{
		return GetBinding(serverAuthentication, namespaceName, TimeSpan.FromMinutes(1.0));
	}

	public static Binding GetBinding(ServerAuthentication serverAuthentication, string namespaceName, TimeSpan timeout)
	{
		Binding binding = null;
		switch (serverAuthentication)
		{
		case ServerAuthentication.OAuthSecure:
			binding = Bindings.GetBasicBinding(transport: true, withCredentials: false, timeout);
			break;
		case ServerAuthentication.OAuthUnsecure:
			binding = Bindings.GetBasicBinding(transport: false, withCredentials: false, timeout);
			break;
		}
		if (binding != null && namespaceName != null)
		{
			binding.Namespace = namespaceName;
		}
		return binding;
	}

	public static void ConfigureEndpoint(ServerAuthentication serverAuthentication, ServiceEndpoint serviceEndpoint, ITokenCache tokenCache)
	{
		if (serviceEndpoint == null)
		{
			throw new ArgumentNullException("serviceEndpoint");
		}
		if (serverAuthentication == ServerAuthentication.OAuthSecure || serverAuthentication == ServerAuthentication.OAuthUnsecure)
		{
			if (tokenCache == null)
			{
				throw new ArgumentNullException("tokenCache");
			}
			serviceEndpoint.EndpointBehaviors.Add(new BearerTokenBehaviour(tokenCache));
		}
		foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
		{
			IOperationBehavior operationBehavior = operation.Behaviors[typeof(DataContractSerializerOperationBehavior)];
			if (operationBehavior != null && operationBehavior is DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = int.MaxValue;
			}
		}
	}

	public static EndpointAddress GetEndpointAddress(ServerAuthentication authenticationType, Uri serverUrl)
	{
		if (authenticationType == ServerAuthentication.OAuthSecure || authenticationType == ServerAuthentication.OAuthUnsecure)
		{
			return new EndpointAddress(serverUrl.AbsoluteUri);
		}
		throw new NotImplementedException($"Authentication type {authenticationType} is not supported");
	}
}
