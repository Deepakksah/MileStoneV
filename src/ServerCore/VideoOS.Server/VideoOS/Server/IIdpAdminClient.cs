using System;

namespace VideoOS.Server;

public interface IIdpAdminClient : IDisposable
{
	void AddAdministrator(string userId);

	void DeleteAdministrator(string userId);

	void DeleteUser(string userId);
}
