using VideoOS.Platform;

namespace VideoOS.GisMapCommunication;

public interface IClientManagerBase<T> where T : IClientBase
{
	T GetClient(ServerId serverId);

	void Close();
}
