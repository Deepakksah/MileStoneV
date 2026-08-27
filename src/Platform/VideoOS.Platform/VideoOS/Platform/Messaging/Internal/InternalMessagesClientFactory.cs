using VideoOS.Platform.Login;

namespace VideoOS.Platform.Messaging.Internal;

public class InternalMessagesClientFactory : IInternalMessagesClientFactory
{
	public static InternalMessagesClientFactory Instance { get; set; }

	public virtual IManagedMessageClient Create(LoginSettings loginSettings)
	{
		throw new MIPException("Method not supported for this Environment");
	}
}
