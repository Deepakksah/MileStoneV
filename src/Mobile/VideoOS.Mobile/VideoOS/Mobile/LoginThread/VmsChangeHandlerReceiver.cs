using System;
using VideoOS.Platform;
using VideoOS.Platform.Messaging;

namespace VideoOS.Mobile.LoginThread;

public class VmsChangeHandlerReceiver : VmsChangeHandler<object>
{
	public VmsChangeHandlerReceiver(string messageId)
		: base(messageId, (Func<object>)(() => (object)null), (Func<object, MessageReceiver, string, object>)((object comm, MessageReceiver handler, string message) => EnvironmentManager.Instance.RegisterReceiver(handler, new MessageIdFilter(message))), (Action<object, object>)delegate(object comm, object receiver)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(receiver);
		})
	{
	}
}
