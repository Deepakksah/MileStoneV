using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging.Internal;

namespace VideoOS.Platform.Common.MessageClient;

public class SdkMessagesClientFactory : InternalMessagesClientFactory
{
	public override IManagedMessageClient Create(LoginSettings loginSettings)
	{
		return new MessageClient(loginSettings);
	}
}
