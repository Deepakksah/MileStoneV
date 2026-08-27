using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.IM.Server.WebApi.Utils;

public class AddTokenBehavior : BehaviorExtensionElement, IEndpointBehavior
{
	private string _accessToken;

	public override Type BehaviorType => typeof(AddTokenBehavior);

	public AddTokenBehavior(string accessToken)
	{
		_accessToken = accessToken;
	}

	public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
	{
		clientRuntime.MessageInspectors.Add(new BearerAuthorizationHeaderInspector(_accessToken));
	}

	public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
	{
	}

	public void Validate(ServiceEndpoint endpoint)
	{
	}

	protected override object CreateBehavior()
	{
		return new AddTokenBehavior(_accessToken);
	}
}
