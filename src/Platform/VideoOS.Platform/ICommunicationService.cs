using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Messaging.Internal;

[GeneratedCode("System.ServiceModel", "3.0.0.0")]
[ServiceContract(ConfigurationName = "ICommunicationService")]
public interface ICommunicationService
{
	[OperationContract(Action = "http://tempuri.org/ICommunicationService/RegisterCommunicationFilter", ReplyAction = "http://tempuri.org/ICommunicationService/RegisterCommunicationFilterResponse")]
	void RegisterCommunicationFilter(string token, CommunicationFilter communicationFilter);

	[OperationContract(Action = "http://tempuri.org/ICommunicationService/UnRegisterCommunicationFilter", ReplyAction = "http://tempuri.org/ICommunicationService/UnRegisterCommunicationFilterResponse")]
	void UnRegisterCommunicationFilter(string token, FQID clientEndPoint, Guid id);

	[OperationContract(Action = "http://tempuri.org/ICommunicationService/TransmitMessage", ReplyAction = "http://tempuri.org/ICommunicationService/TransmitMessageResponse")]
	void TransmitMessage(string token, FQID clientEndPoint, ExternalMessage message);

	[OperationContract(Action = "http://tempuri.org/ICommunicationService/Receive", ReplyAction = "http://tempuri.org/ICommunicationService/ReceiveResponse")]
	[FaultContract(typeof(MessageFilterNotRegisteredMIPException))]
	[ServiceKnownType(typeof(Guid))]
	[ServiceKnownType(typeof(ExternalMessage))]
	[ServiceKnownType(typeof(CommunicationFilter))]
	object[] Receive(string token, FQID clientEndPoint, int timeoutMilliSeconds);
}
