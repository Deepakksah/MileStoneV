using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace VideoOS.GisMapSharedData.CustomHeader;

public class CustomHeaderMessageInspector : IClientMessageInspector
{
	private IEnumerable<Func<MessageHeader>> _factoryMethods;

	public CustomHeaderMessageInspector(IEnumerable<Func<MessageHeader>> factoryMethods)
	{
		_factoryMethods = factoryMethods;
	}

	public object BeforeSendRequest(ref Message request, IClientChannel channel)
	{
		Message message = request;
		if (message == null)
		{
			throw new ArgumentNullException("request");
		}
		foreach (Func<MessageHeader> factoryMethod in _factoryMethods)
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
