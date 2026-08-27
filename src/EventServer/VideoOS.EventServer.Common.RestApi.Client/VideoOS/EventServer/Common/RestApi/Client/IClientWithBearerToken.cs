using System;

namespace VideoOS.EventServer.Common.RestApi.Client;

public interface IClientWithBearerToken
{
	string BearerToken { get; set; }

	Func<string> BearerTokenProvider { get; set; }
}
