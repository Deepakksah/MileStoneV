namespace VideoOS.Server;

public interface ISessionDataManager
{
	void Store(SessionData sessionData);

	SessionData Query(long sessionDataId);

	void Remove(SessionData sessionData);
}
