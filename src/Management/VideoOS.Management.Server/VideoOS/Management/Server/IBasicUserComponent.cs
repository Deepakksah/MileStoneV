using System;

namespace VideoOS.Management.Server;

public interface IBasicUserComponent : IDisposable
{
	void DeleteBasicUser(string id, string sid, string userName, Guid sessionId);
}
