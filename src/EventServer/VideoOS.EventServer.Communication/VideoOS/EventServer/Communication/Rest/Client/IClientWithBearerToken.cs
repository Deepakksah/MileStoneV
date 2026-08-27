using System;

namespace VideoOS.EventServer.Communication.Rest.Client;

public interface IClientWithBearerToken
{
	string BearerToken { get; set; }

	Func<string> BearerTokenProvider { get; set; }
}
