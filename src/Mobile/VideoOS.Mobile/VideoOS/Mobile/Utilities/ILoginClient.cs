using System;
using System.Net;

namespace VideoOS.Mobile.Utilities;

public interface ILoginClient
{
	Uri Url { get; set; }

	NetworkCredential Credentials { get; set; }

	void Login(Guid instanceId, string currentToken);
}
