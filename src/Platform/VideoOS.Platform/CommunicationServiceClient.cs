using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Messaging.Internal;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "3.0.0.0")]
public class CommunicationServiceClient : ClientBase<ICommunicationService>, ICommunicationService
{
	public CommunicationServiceClient()
	{
	}

	public CommunicationServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public CommunicationServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public CommunicationServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public CommunicationServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public void RegisterCommunicationFilter(string token, CommunicationFilter communicationFilter)
	{
		base.Channel.RegisterCommunicationFilter(token, communicationFilter);
	}

	public void UnRegisterCommunicationFilter(string token, FQID clientEndPoint, Guid id)
	{
		base.Channel.UnRegisterCommunicationFilter(token, clientEndPoint, id);
	}

	public void TransmitMessage(string token, FQID destinationEndPoint, ExternalMessage message)
	{
		base.Channel.TransmitMessage(token, destinationEndPoint, message);
	}

	public object[] Receive(string token, FQID clientEndPoint, int timeoutMilliSeconds)
	{
		return base.Channel.Receive(token, clientEndPoint, timeoutMilliSeconds);
	}
}
