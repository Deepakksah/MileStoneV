namespace VideoOS.Platform.SDK.Facades;

public interface IServerOfflineCacheFacade
{
	void Clear();

	void Remove(string serverAddress);
}
