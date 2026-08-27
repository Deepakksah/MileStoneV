using System;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Management.Server;

public class BasicAuthenticationInvoker : IOperationInvoker
{
	private readonly IOperationInvoker defaultInvoker;

	public bool IsSynchronous => defaultInvoker.IsSynchronous;

	public BasicAuthenticationInvoker(IOperationInvoker defaultInvoker)
	{
		if (defaultInvoker == null)
		{
			throw new ArgumentNullException("defaultInvoker");
		}
		this.defaultInvoker = defaultInvoker;
	}

	public object[] AllocateInputs()
	{
		return defaultInvoker.AllocateInputs();
	}

	public object Invoke(object instance, object[] inputs, out object[] outputs)
	{
		if (MessagePropertiesManager.IncomingMessageProperties.ContainsKey("BasicAuthenticationResult") && !(bool)MessagePropertiesManager.IncomingMessageProperties["BasicAuthenticationResult"])
		{
			outputs = null;
			return null;
		}
		return defaultInvoker.Invoke(instance, inputs, out outputs);
	}

	public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
	{
		if (MessagePropertiesManager.IncomingMessageProperties.ContainsKey("BasicAuthenticationResult") && !(bool)MessagePropertiesManager.IncomingMessageProperties["BasicAuthenticationResult"])
		{
			return null;
		}
		return defaultInvoker.InvokeBegin(instance, inputs, callback, state);
	}

	public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
	{
		return defaultInvoker.InvokeEnd(instance, out outputs, result);
	}
}
