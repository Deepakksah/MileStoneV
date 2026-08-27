using System;
using VideoOS.Platform;

namespace VideoOS.Mobile.LoginThread;

public interface ILoginData
{
	string Username { get; }

	string Password { get; }

	bool AdUser { get; }

	Uri ServerUri { get; }

	bool IsSecured { get; }

	FQID ServerFqid { get; set; }

	Guid XpmoInstance { get; }
}
