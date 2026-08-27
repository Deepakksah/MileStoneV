using VideoOS.Platform.Login;

namespace VideoOS.Platform.Messaging.Internal;

public interface IInternalMessagesClientFactory
{
	IManagedMessageClient Create(LoginSettings loginSettings);
}
