using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Management.Server;

public class BasicAuthenticationInvokerBehavior : IOperationBehavior
{
	public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
	{
	}

	public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
	{
	}

	public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
	{
		if (dispatchOperation == null)
		{
			throw new ArgumentNullException("dispatchOperation");
		}
		IOperationInvoker invoker = dispatchOperation.Invoker;
		dispatchOperation.Invoker = new BasicAuthenticationInvoker(invoker);
	}

	public void Validate(OperationDescription operationDescription)
	{
	}
}
