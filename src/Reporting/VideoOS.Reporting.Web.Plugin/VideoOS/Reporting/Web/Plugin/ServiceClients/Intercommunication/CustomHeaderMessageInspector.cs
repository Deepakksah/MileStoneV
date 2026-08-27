using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.Reporting.Web.Plugin.ServiceClients.Intercommunication;

public class CustomHeaderMessageInspector : IClientMessageInspector
{
	private IEnumerable<MessageHeaderFactoryMethod> _factoryMethods;

	public CustomHeaderMessageInspector(IEnumerable<MessageHeaderFactoryMethod> factoryMethods)
	{
		_factoryMethods = factoryMethods;
	}

	public object BeforeSendRequest(ref Message request, IClientChannel channel)
	{
		Message message = request;
		if (message == null)
		{
			throw new ArgumentException("request was null");
		}
		foreach (MessageHeaderFactoryMethod factoryMethod in _factoryMethods)
		{
			MessageHeader messageHeader = factoryMethod();
			if (messageHeader != null)
			{
				message.Headers.Add(messageHeader);
			}
		}
		return null;
	}

	public void AfterReceiveReply(ref Message reply, object correlationState)
	{
	}
}
