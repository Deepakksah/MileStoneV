using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Facades;

public interface IServerTypeCacheFacade
{
	void RemoveAllServers();

	void RemoveServer(LoginSettings loginSettings);
}
