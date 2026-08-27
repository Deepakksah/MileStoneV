using System;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace VideoOS.Mobile.LoginThread;

public class VmsChangeHandlerCommunication : VmsChangeHandler<MessageCommunication>
{
	public VmsChangeHandlerCommunication(string messageId)
		: base(messageId, (Func<MessageCommunication>)delegate
		{
			MessageCommunicationManager.Start(EnvironmentManager.Instance.MasterSite.ServerId);
			return MessageCommunicationManager.Get(EnvironmentManager.Instance.MasterSite.ServerId);
		}, (Func<MessageCommunication, MessageReceiver, string, object>)((MessageCommunication comm, MessageReceiver handler, string message) => comm.RegisterCommunicationFilter(handler, new CommunicationIdFilter(message))), (Action<MessageCommunication, object>)delegate(MessageCommunication comm, object receiver)
		{
			comm.UnRegisterCommunicationFilter(receiver);
		})
	{
	}
}
