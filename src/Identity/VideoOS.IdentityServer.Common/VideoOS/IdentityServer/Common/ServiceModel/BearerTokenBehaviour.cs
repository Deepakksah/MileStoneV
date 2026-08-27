using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using VideoOS.IdentityServer.Common.Handlers;

namespace VideoOS.IdentityServer.Common.ServiceModel;

public class BearerTokenBehaviour : IEndpointBehavior
{
	private readonly ITokenCache _tokenCache;

	public BearerTokenBehaviour(ITokenCache tokenCache)
	{
		_tokenCache = tokenCache;
	}

	public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
		if (_tokenCache != null)
		{
			BearerAuthorizationHeaderInspector item = new BearerAuthorizationHeaderInspector(_tokenCache);
			clientRuntime.ClientMessageInspectors.Add(item);
		}
	}

	public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
	}

	public void Validate(ServiceEndpoint endpoint)
	{
	}
}
